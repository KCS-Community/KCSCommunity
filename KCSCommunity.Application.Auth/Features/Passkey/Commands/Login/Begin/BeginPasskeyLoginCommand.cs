using Fido2NetLib;
using MediatR;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Begin;
public record BeginPasskeyLoginCommand(string? UserName) : IRequest<AssertionOptions>;