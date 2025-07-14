using AutoMapper;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KCSCommunity.Application.Shared.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //User
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        //Role
        CreateMap<IdentityRole<Guid>, RoleDto>();
        
        //Passkey
        CreateMap<PasskeyCredential, PasskeyDto>()
            .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegDate));
    }
}