using MediatR;
namespace KCSCommunity.Application.Features.Authorization.Commands.RevokeToken;
public record RevokeTokenCommand(Guid UserId) : IRequest;