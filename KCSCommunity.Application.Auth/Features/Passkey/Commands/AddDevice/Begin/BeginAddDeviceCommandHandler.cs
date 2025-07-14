using System.Security.Claims;
using Fido2NetLib;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Begin;

public class BeginAddDeviceCommandHandler : IRequestHandler<BeginAddDeviceCommand, CredentialCreateOptions>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasskeyService _passkeyService;
    private readonly ISessionStore _sessionStore;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BeginAddDeviceCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, ISessionStore sessionStore, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context; _passkeyService = passkeyService; _sessionStore = sessionStore; _userManager = userManager; _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CredentialCreateOptions> Handle(BeginAddDeviceCommand request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? throw new UnauthorizedAccessException();
        
        var user = await _userManager.FindByIdAsync(userIdString)
                   ?? throw new NotFoundException("User", userIdString);
        
        var existingCredentials = await _context.PasskeyCredentials
            .Where(c => c.UserId == user.Id)
            .ToListAsync(cancellationToken);
        var options = _passkeyService.GenerateRegistrationOptions(user, request.DeviceName, existingCredentials);
        
        var sessionData = new Fido2SessionData { 
            CreateOptions = options,
            //RegistrationUserName = user.UserName,
            //RegistrationNickname = user.Nickname,
            //RegistrationAvatarUrl = user.AvatarUrl,
            RegistrationDeviceName = request.DeviceName
        };
        _sessionStore.Set(FidoConstants.Fido2OptionsKey, sessionData);

        return options;
    }
}