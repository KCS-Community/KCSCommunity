namespace KCSCommunity.Infrastructure.Security.Jwt;

/// <summary>
/// Represents the JWT settings from appsettings.json.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
}