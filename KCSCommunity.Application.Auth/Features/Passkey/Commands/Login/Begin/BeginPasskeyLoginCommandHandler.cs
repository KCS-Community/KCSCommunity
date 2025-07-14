using Fido2NetLib;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Domain.Constants;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Begin;
public class BeginPasskeyLoginCommandHandler : IRequestHandler<BeginPasskeyLoginCommand, AssertionOptions>
{
    private readonly IPasskeyService _passkeyService;
    private readonly ISessionStore _sessionStore;

    public BeginPasskeyLoginCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, ISessionStore sessionStore)
    {
        _passkeyService = passkeyService;
        _sessionStore = sessionStore;
    }

    public Task<AssertionOptions> Handle(BeginPasskeyLoginCommand request, CancellationToken cancellationToken)
    {
        var options = _passkeyService.GenerateLoginOptions();
        
        var sessionData = new Fido2SessionData { AssertionOptions = options };
        _sessionStore.Set(FidoConstants.Fido2OptionsKey, sessionData);
        
        return Task.FromResult(options);
    }
}