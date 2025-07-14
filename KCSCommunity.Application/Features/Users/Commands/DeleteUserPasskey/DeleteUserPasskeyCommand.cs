using MediatR;
namespace KCSCommunity.Application.Features.Users.Commands.DeleteUserPasskey;

public record DeleteUserPasskeyCommand(Guid UserId, string PasskeyId) : IRequest;