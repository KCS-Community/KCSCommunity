using FluentValidation.Results;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Complete;

public class CompletePasskeyActivationCommandHandler : IRequestHandler<CompletePasskeyActivationCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasskeyService _passkeyService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceLockService _lockService;
    private readonly ISessionStore _sessionStore;
    private readonly ILogger<CompletePasskeyActivationCommandHandler> _logger;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;
    private readonly IPasskeyOptionsValidator _passkeyOptionsValidator;

    public CompletePasskeyActivationCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, UserManager<ApplicationUser> userManager, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, IResourceLockService lockService, ILogger<CompletePasskeyActivationCommandHandler> logger, IPasskeyOptionsValidator passkeyOptionsValidator)
    { _context = context; _passkeyService = passkeyService; _userManager = userManager; _sessionStore = sessionStore;
        _localizer = localizer;
        _lockService = lockService;
        _logger = logger;
        _passkeyOptionsValidator = passkeyOptionsValidator;
    }

    public async Task Handle(CompletePasskeyActivationCommand request, CancellationToken cancellationToken)
    {
        var sessionData = _sessionStore.Get<Fido2SessionData>(FidoConstants.Fido2OptionsKey); 
        _passkeyOptionsValidator.Validate(sessionData);
        
        var registrationPasscode = sessionData!.RegistrationPasscode;

        _sessionStore.Remove(FidoConstants.Fido2OptionsKey);
        
        var passcode = await _context.OneTimePasscodes.SingleOrDefaultAsync(p => p.Code == registrationPasscode && !p.IsUsed && p.ExpiryDate > DateTime.UtcNow , cancellationToken);
        if (passcode == null)
        {
            throw new ValidationException(new[]
            { 
                new ValidationFailure("Passcode", 
                    _localizer["ActivateUserInvalidOrExpiredPasscode"])
            });
        }

        var user = await _userManager.FindByIdAsync(passcode.UserId.ToString());
        if (user == null) throw new ValidationException(new[] { new ValidationFailure("UserName", _localizer["ActivateUserInvalidUser"]) });

        if (user.IsActive) throw new ValidationException(new[] { new ValidationFailure("UserName", _localizer["ActivateUserAlreadyActive"]) });
        
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
        
        var newCredential = await _passkeyService.CompleteRegistrationAsync(request.AttestationResponse, sessionData.CreateOptions!, user.Id, sessionData.RegistrationDeviceName!);
        await _userManager.SetLockoutEndDateAsync(user, null);
        await _context.PasskeyCredentials.AddAsync(newCredential, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}