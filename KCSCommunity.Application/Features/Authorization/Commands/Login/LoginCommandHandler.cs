using System.IdentityModel.Tokens.Jwt;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Common.Exceptions;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using KCSCommunity.Abstractions.Models.Configuration;
using KCSCommunity.Application.Resources;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Features.Authorization.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IApplicationDbContext context,
        JwtSettings jwtSettings,
        IStringLocalizer<SharedValidationMessages> localizer)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _context = context;
        _jwtSettings = jwtSettings;
        _localizer = localizer;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            throw new ValidationException(new[] { new ValidationFailure("Login", _localizer["LoginInvalidUserOrPassword"]) });
        }
        
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var message = lockoutEnd.HasValue 
                ? _localizer["LoginLockedOut", lockoutEnd.Value.ToLocalTime().ToString("F")]
                : _localizer["LoginLockedOutPermanently"];
            throw new ValidationException(new[] { new ValidationFailure("Login", message) });
        }

        if (!user.IsActive)
        {
            var message = _localizer["LoginAccountNotActive"];
            throw new ValidationException(new[] { new ValidationFailure("Login", message) });
        }

        if (user.TimeoutEndDate.HasValue && user.TimeoutEndDate.Value > DateTime.UtcNow)
        {
            var message = _localizer["LoginTimedOut",user.TimeoutEndDate.Value.ToLocalTime().ToString("F")];
            throw new ValidationException(new[] { new ValidationFailure("Login", message) });
        }

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
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
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