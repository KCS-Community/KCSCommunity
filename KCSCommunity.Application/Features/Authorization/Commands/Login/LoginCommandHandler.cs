using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Common.Exceptions;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;

namespace KCSCommunity.Application.Features.Authorization.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
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

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        return new LoginResponse(token);
    }
}