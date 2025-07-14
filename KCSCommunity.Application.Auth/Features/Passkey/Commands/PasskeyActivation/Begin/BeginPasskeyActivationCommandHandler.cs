using Fido2NetLib;
using FluentValidation.Results;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Begin;

public class BeginPasskeyActivationCommandHandler : IRequestHandler<BeginPasskeyActivationCommand, CredentialCreateOptions>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasskeyService _passkeyService;
    private readonly ISessionStore _sessionStore;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public BeginPasskeyActivationCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, UserManager<ApplicationUser> userManager)
    { _context = context; _passkeyService = passkeyService; _sessionStore = sessionStore;
        _localizer = localizer;
        _userManager = userManager;
    }

    public async Task<CredentialCreateOptions> Handle(BeginPasskeyActivationCommand request, CancellationToken cancellationToken)
    {
        var passcode = await _context.OneTimePasscodes.SingleOrDefaultAsync(p => p.Code == request.Passcode && !p.IsUsed && p.ExpiryDate > DateTime.UtcNow , cancellationToken);
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
        
        var existingCredentials = await _context.PasskeyCredentials
            .Where(c => c.UserId == user.Id)
            .ToListAsync(cancellationToken);
        
        var options = _passkeyService.GenerateRegistrationOptions(user, request.DeviceName, existingCredentials);
        
        var sessionData = new Fido2SessionData { 
            CreateOptions = options, 
            //RegistrationUserName = request.UserName,
            //RegistrationNickname = request.Nickname,
            //RegistrationAvatarUrl = request.AvatarUrl,
            RegistrationDeviceName = request.DeviceName,
            RegistrationPasscode = request.Passcode
        };
        _sessionStore.Set(FidoConstants.Fido2OptionsKey, sessionData);

        return options;
    }
}