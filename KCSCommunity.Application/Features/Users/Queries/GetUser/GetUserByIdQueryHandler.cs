using AutoMapper;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace KCSCommunity.Application.Features.Users.Queries.GetUser;
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        
        var userDto = _mapper.Map<UserDto>(user);
        var roles = await _userManager.GetRolesAsync(user);
        return userDto with { Roles = roles };
    }
}