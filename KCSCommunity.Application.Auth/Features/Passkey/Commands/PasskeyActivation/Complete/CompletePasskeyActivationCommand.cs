using Fido2NetLib;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Complete;

public record CompletePasskeyActivationCommand(AuthenticatorAttestationRawResponse AttestationResponse) : IRequest;