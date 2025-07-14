using KCSCommunity.Abstractions.Models.Dtos;
using MediatR;
namespace KCSCommunity.Application.Features.Roles.Queries.GetAllRoles;
public record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;