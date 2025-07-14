using KCSCommunity.Domain.Enums;
using MediatR;
namespace KCSCommunity.Application.Features.Users.Commands.CreateUser;
public record CreateUserCommand(string UserName, string RealName, string? EnglishName, UserRoleType RoleType, string? Grade, string? House, string? StaffTitle) : IRequest<CreateUserResponse>;