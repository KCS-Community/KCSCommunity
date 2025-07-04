using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using KCSCommunity.Domain.Entities;

namespace KCSCommunity.Application.Features.Users.Commands.ActivateUser;
public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceLockService _lockService;
    private readonly ILogger<ActivateUserCommandHandler> _logger;

    public ActivateUserCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager, IResourceLockService lockService, ILogger<ActivateUserCommandHandler> logger)
    {
        _context = context; _userManager = userManager; _lockService = lockService; _logger = logger;
    }

    public async Task Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null) throw new ValidationException(new[] { new ValidationFailure("UserName", "Invalid user.") });
        
        string lockKey = $"activate-user-{user.Id}";
        using var userLock = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10), cancellationToken);

        if (userLock == null)
        {
            _logger.LogWarning("Could not acquire lock for user activation: {UserName}.", request.UserName);
            throw new InvalidOperationException("This account is currently being processed. Please try again in a moment.");
        }

        user = await _userManager.FindByIdAsync(user.Id.ToString()); // Re-fetch user state inside the lock
        if (user!.IsActive) throw new ValidationException(new[] { new ValidationFailure("UserName", "Account already active.") });
        
        var passcode = await _context.OneTimePasscodes.SingleOrDefaultAsync(p => p.Code == request.Passcode && !p.IsUsed && p.ExpiryDate > DateTime.UtcNow && p.UserId == user.Id, cancellationToken);
        if (passcode == null) throw new ValidationException(new[] { new ValidationFailure("Passcode", "Invalid or expired passcode.") });

        passcode.MarkAsUsed();
        user.ActivateAccount(request.Nickname, request.AvatarUrl);

        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
        if(!removePasswordResult.Succeeded) throw new Exception("Failed to remove temporary password during activation.");

        var addPasswordResult = await _userManager.AddPasswordAsync(user, request.Password);
        if (!addPasswordResult.Succeeded) throw new ValidationException(addPasswordResult.Errors.Select(e => new ValidationFailure(e.Code, e.Description)));

        await _userManager.SetLockoutEndDateAsync(user, null);
        await _context.SaveChangesAsync(cancellationToken);
    }
}