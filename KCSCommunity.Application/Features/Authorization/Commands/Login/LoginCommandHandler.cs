using System.IdentityModel.Tokens.Jwt;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Common.Exceptions;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using KCSCommunity.Abstractions.Models.Configuration;

namespace KCSCommunity.Application.Features.Authorization.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IApplicationDbContext context,          // 新增
        JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _context = context;
        _jwtSettings = jwtSettings;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            throw new ValidationException(new[] { new ValidationFailure("Login", "Invalid username or password.") });
        }
        
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var message = lockoutEnd.HasValue 
                ? $"This account has been locked out. Please try again after {lockoutEnd.Value.ToLocalTime():F}."
                : "This account has been locked out.";
            throw new ValidationException(new[] { new ValidationFailure("Login", message) });
        }

        if (!user.IsActive)
        {
            throw new ValidationException(new[] { new ValidationFailure("Login", "Account is not active. Please activate your account first.") });
        }

        if (user.TimeoutEndDate.HasValue && user.TimeoutEndDate.Value > DateTime.UtcNow)
        {
            throw new ValidationException(new[] { new ValidationFailure("Login", $"This account is timed out until {user.TimeoutEndDate.Value:F}.") });
        }

        if (user.PasswordHash == null)
        {
            await _userManager.AccessFailedAsync(user);
            throw new ValidationException(new[] { new ValidationFailure("Login", "Invalid username or password.") });
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            await _userManager.AccessFailedAsync(user);
            throw new ValidationException(new[] { new ValidationFailure("Login", "Invalid username or password.") });
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