using Fido2NetLib;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Complete;

public record CompleteAddDeviceCommand(AuthenticatorAttestationRawResponse AttestationResponse) : IRequest;