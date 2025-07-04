using AutoMapper;
using KCSCommunity.Application.Features.Roles.Queries.GetAllRoles;
using KCSCommunity.Application.Features.Users.Queries.GetUser;
using KCSCommunity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KCSCommunity.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //User
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        //Role
        CreateMap<IdentityRole<Guid>, RoleDto>();
    }
}