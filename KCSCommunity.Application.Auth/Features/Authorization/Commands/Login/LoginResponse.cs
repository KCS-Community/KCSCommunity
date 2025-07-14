namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.Login;

public record LoginResponse(string AccessToken, string RefreshToken, Guid UserId);