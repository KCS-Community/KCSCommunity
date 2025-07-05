using System.ComponentModel.DataAnnotations;

namespace KCSCommunity.Abstractions.Models.Configuration;
public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    
    [Required]
    public string Secret { get; init; } = null!;
    
    [Required]
    public string Issuer { get; init; } = null!;
    
    [Required]
    public string Audience { get; init; } = null!;
    
    [Range(1, int.MaxValue)]
    public int ExpiryMinutes { get; init; }
    
    [Range(1, int.MaxValue)]
    public int RefreshTokenExpiryDays { get; init; }
}