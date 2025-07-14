using MediatR;
namespace KCSCommunity.Application.Features.Users.Commands.ActivateUser;
public record ActivateUserCommand(string Passcode,/* string UserName, string? Nickname, string? AvatarUrl,*/ string Password) : IRequest;