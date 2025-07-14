using Fido2NetLib;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Begin;

public record BeginAddDeviceCommand(string DeviceName) : IRequest<CredentialCreateOptions>;