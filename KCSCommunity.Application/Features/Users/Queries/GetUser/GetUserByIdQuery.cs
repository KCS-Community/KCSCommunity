using MediatR;
namespace KCSCommunity.Application.Features.Users.Queries.GetUser;
public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;