using System.IdentityModel.Tokens.Jwt;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Auth.Features.Authorization.Commands.Login;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Complete;
public class CompletePasskeyLoginCommandHandler : IRequestHandler<CompletePasskeyLoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasskeyService _passkeyService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IAuthTokenSettings _tokenSettings;
    private readonly ISessionStore _sessionStore;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;
    private readonly IUserStatusValidator _userStatusValidator;

    public CompletePasskeyLoginCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, UserManager<ApplicationUser> userManager, IJwtService jwtService, IAuthTokenSettings tokenSettings, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, IUserStatusValidator userStatusValidator)
    { _context = context; _passkeyService = passkeyService; _userManager = userManager; _jwtService = jwtService; _tokenSettings = tokenSettings;
        _sessionStore = sessionStore;
        _localizer = localizer;
        _userStatusValidator = userStatusValidator;
    }

    public async Task<LoginResponse> Handle(CompletePasskeyLoginCommand request, CancellationToken cancellationToken)
    {
        var sessionData = _sessionStore.Get<Fido2SessionData>(FidoConstants.Fido2OptionsKey);
        if (sessionData?.AssertionOptions == null)
        {
            throw new InvalidOperationException(_localizer["PasskeyOptionsNotFound"]);
        }
        _sessionStore.Remove(FidoConstants.Fido2OptionsKey);
        var userId = await _passkeyService.CompleteLoginAsync(request.ClientResponse, sessionData.AssertionOptions);
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new NotFoundException(_localizer["PasskeyUserNotFound"]);
        }
        
        await _userStatusValidator.ValidateAsync(user);
        
        await _userManager.ResetAccessFailedCountAsync(user);
        
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user, roles);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var jti = jwtToken.Id;

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = RefreshToken.GenerateTokenValue(),
                JwtId = jti,
                ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.GetRefreshTokenExpiryDays())
            };
            
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new LoginResponse(accessToken, refreshToken.Token, user.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}