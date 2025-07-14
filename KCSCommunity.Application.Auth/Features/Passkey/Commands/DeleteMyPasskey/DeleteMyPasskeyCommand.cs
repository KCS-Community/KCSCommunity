using MediatR;
namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.DeleteMyPasskey;

public record DeleteMyPasskeyCommand(string PasskeyId) : IRequest;