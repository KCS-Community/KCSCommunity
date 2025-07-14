using MediatR;

namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.Login;

public record LoginCommand(string UserName, string Password) : IRequest<LoginResponse>;