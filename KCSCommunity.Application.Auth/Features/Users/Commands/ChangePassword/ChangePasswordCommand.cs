using MediatR;
namespace KCSCommunity.Application.Auth.Features.Users.Commands.ChangePassword;
public record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest;