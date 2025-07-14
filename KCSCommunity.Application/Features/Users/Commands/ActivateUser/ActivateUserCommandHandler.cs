using KCSCommunity.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Domain.Entities;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Features.Users.Commands.ActivateUser;
public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceLockService _lockService;
    private readonly ILogger<ActivateUserCommandHandler> _logger;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public ActivateUserCommandHandler(IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IResourceLockService lockService,
        ILogger<ActivateUserCommandHandler> logger,
        IStringLocalizer<SharedValidationMessages> localizer)
    {
        _context = context;
        _userManager = userManager;
        _lockService = lockService;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var passcode = await _context.OneTimePasscodes.SingleOrDefaultAsync(p => p.Code == request.Passcode && !p.IsUsed && p.ExpiryDate > DateTime.UtcNow /*&& p.UserId == user.Id*/, cancellationToken);
        if (passcode == null) throw new ValidationException(new[] { new ValidationFailure("Passcode", _localizer["ActivateUserInvalidOrExpiredPasscode"]) });

        var user = await _userManager.FindByIdAsync(passcode.UserId.ToString());
        if (user == null) throw new ValidationException(new[] { new ValidationFailure("UserName", _localizer["ActivateUserInvalidUser"]) });
        
        string lockKey = $"activate-user-{user.Id}";
        using var userLock = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10), cancellationToken);

        if (userLock == null)
        {
            _logger.LogWarning("Could not acquire lock for user activation: {UserName}.", user.UserName);
            throw new InvalidOperationException(_localizer["ActivateUserBusy"]);
        }

        user = await _userManager.FindByIdAsync(user.Id.ToString()); // Re-fetch user state inside the lock
        if (user!.IsActive) throw new ValidationException(new[] { new ValidationFailure("UserName", _localizer["ActivateUserAlreadyActive"]) });
        
        passcode.MarkAsUsed();
        user.ActivateAccount();

        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
        if(!removePasswordResult.Succeeded) throw new Exception(_localizer["ActivateUserFailedToRemoveTemporaryPassword"]);

        var addPasswordResult = await _userManager.AddPasswordAsync(user, request.Password);
        if (!addPasswordResult.Succeeded) throw new ValidationException(addPasswordResult.Errors.Select(e => new ValidationFailure(e.Code, e.Description)));

        await _userManager.SetLockoutEndDateAsync(user, null);
        await _context.SaveChangesAsync(cancellationToken);
    }
}