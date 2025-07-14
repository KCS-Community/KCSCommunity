using MediatR;
namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.RevokeToken;
public record RevokeTokenCommand(Guid UserId) : IRequest;