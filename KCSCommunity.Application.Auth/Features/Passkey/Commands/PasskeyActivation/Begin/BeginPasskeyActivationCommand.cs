using Fido2NetLib;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Begin;

public record BeginPasskeyActivationCommand(
    //string UserName,
    //string? Nickname,
    //string? AvatarUrl,
    string Passcode,
    string DeviceName
) : IRequest<CredentialCreateOptions>;