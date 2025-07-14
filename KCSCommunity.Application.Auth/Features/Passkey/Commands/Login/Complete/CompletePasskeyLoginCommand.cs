using Fido2NetLib;
using KCSCommunity.Application.Auth.Features.Authorization.Commands.Login;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Complete;
public record CompletePasskeyLoginCommand(AuthenticatorAssertionRawResponse ClientResponse) : IRequest<LoginResponse>;