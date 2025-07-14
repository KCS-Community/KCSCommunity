using KCSCommunity.Abstractions.Interfaces;

namespace KCSCommunity.Abstractions.Models.Configuration;
public class JwtSettings : IAuthTokenSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int AccessTokenExpiryMinutes { get; init; }
    public int RefreshTokenExpiryDays { get; init; }

    public string GetSecret() => Secret;

    public string GetIssuer() => Issuer;

    public string GetAudience() => Audience;

    public int GetAccessTokenExpiryMinutes() => AccessTokenExpiryMinutes;

    public int GetRefreshTokenExpiryDays() => RefreshTokenExpiryDays;
}