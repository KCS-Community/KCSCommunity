using MediatR;
namespace KCSCommunity.Application.Features.Roles.Commands.CreateRole;
public record CreateRoleCommand(string RoleName) : IRequest<Guid>;