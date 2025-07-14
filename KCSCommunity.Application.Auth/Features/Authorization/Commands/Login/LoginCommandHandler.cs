using System.IdentityModel.Tokens.Jwt;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Application.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IApplicationDbContext _context;
    private readonly IAuthTokenSettings _tokenSettings;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;
    private readonly IUserStatusValidator _userStatusValidator;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IApplicationDbContext context,
        IAuthTokenSettings tokenSettings,
        IStringLocalizer<SharedValidationMessages> localizer, IUserStatusValidator userStatusValidator)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _context = context;
        _tokenSettings = tokenSettings;
        _localizer = localizer;
        _userStatusValidator = userStatusValidator;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            throw new ValidationException(new[] { new ValidationFailure("Login", _localizer["LoginInvalidUserOrPassword"]) });
        }
        
        await _userStatusValidator.ValidateAsync(user);

        if (user.PasswordHash == null)
        {
            await _userManager.AccessFailedAsync(user);
            throw new ValidationException(new[] { new ValidationFailure("Login", _localizer["LoginInvalidUserOrPassword"]) });
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            await _userManager.AccessFailedAsync(user);
            throw new ValidationException(new[] { new ValidationFailure("Login", _localizer["LoginInvalidUserOrPassword"]) });
        }
        
        //password was correct
        if (passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            //rehash and update the users password hash.
            var newHash = _passwordHasher.HashPassword(user, request.Password);
            user.PasswordHash = newHash;
            //persist this change.
            await _userManager.UpdateAsync(user); 
        }
        
        await _userManager.ResetAccessFailedCountAsync(user);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user, roles);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var jti = jwtToken.Id;

            var refreshToken = new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                Token = Domain.Entities.RefreshToken.GenerateTokenValue(),
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