using MediatR;
namespace KCSCommunity.Application.Features.Roles.Commands.UpdateUsersInRole;
public record UpdateUsersInRoleCommand(Guid RoleId, List<Guid> UserIds) : IRequest;