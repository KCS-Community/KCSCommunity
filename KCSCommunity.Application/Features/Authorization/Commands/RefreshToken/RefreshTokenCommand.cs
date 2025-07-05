using KCSCommunity.Application.Features.Authorization.Commands.Login;
using MediatR;
namespace KCSCommunity.Application.Features.Authorization.Commands.RefreshToken;
public record RefreshTokenCommand(string ExpiredAccessToken, string RefreshToken) : IRequest<LoginResponse>;