using KCSCommunity.Domain.Enums;
using MediatR;
namespace KCSCommunity.Application.Features.Users.Commands.CreateUser;
public record CreateUserCommand(string UserName, string Email, string RealName, string? EnglishName, Gender Gender, DateTime DateOfBirth, UserRoleType RoleType, string? Grade, string? House, string? StaffTitle) : IRequest<CreateUserResponse>;