using AutoMapper;
using KCSCommunity.Abstractions.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace KCSCommunity.Application.Features.Roles.Queries.GetAllRoles;
public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IMapper _mapper;
    public GetAllRolesQueryHandler(RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper)
    {
        _roleManager = roleManager; _mapper = mapper;
    }
    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.AsNoTracking().OrderBy(r => r.Name).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}