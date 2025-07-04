using MediatR;
namespace KCSCommunity.Application.Features.Roles.Commands.DeleteRole;
public record DeleteRoleCommand(Guid RoleId) : IRequest;