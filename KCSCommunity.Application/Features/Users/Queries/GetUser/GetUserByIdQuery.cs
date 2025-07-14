using KCSCommunity.Abstractions.Models.Dtos;
using MediatR;
namespace KCSCommunity.Application.Features.Users.Queries.GetUser;
public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;