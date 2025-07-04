using AutoMapper;
using KCSCommunity.Application.Common.Exceptions;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace KCSCommunity.Application.Features.Users.Queries.GetUser;
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    { _userManager = userManager; _mapper = mapper; }
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.Id);
        
        var roles = await _userManager.GetRolesAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        return userDto with { Roles = roles };
    }
}