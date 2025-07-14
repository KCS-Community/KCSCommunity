using System.Security.Claims;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Complete;

public class CompleteAddDeviceCommandHandler : IRequestHandler<CompleteAddDeviceCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasskeyService _passkeyService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISessionStore _sessionStore;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public CompleteAddDeviceCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, UserManager<ApplicationUser> userManager, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, IHttpContextAccessor httpContextAccessor)
    { _context = context; _passkeyService = passkeyService; _userManager = userManager; _sessionStore = sessionStore;
        _localizer = localizer;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(CompleteAddDeviceCommand request, CancellationToken cancellationToken)
    {
        var sessionData = _sessionStore.Get<Fido2SessionData>(FidoConstants.Fido2OptionsKey);
        if (sessionData?.CreateOptions == null || string.IsNullOrEmpty(sessionData.RegistrationDeviceName))
        {
            throw new InvalidOperationException(_localizer["PasskeyOptionsNotFound"]);
        }

        _sessionStore.Remove(FidoConstants.Fido2OptionsKey);
        
        var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) throw new UnauthorizedAccessException();

        var user = await _userManager.FindByIdAsync(userIdStr);
        if (user == null) throw new UnauthorizedAccessException();
        
        var newCredential = await _passkeyService.CompleteRegistrationAsync(request.AttestationResponse, sessionData.CreateOptions, user.Id, sessionData.RegistrationDeviceName!);
        await _context.PasskeyCredentials.AddAsync(newCredential, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}