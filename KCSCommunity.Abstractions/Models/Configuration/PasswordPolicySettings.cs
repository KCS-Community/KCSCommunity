using System.ComponentModel.DataAnnotations;

namespace KCSCommunity.Abstractions.Models.Configuration;

public class PasswordPolicySettings
{
    public const string SectionName = "PasswordPolicy";

    [Range(6, 128)]
    public int RequiredLength { get; set; } = 8;
    
    public bool RequireDigit { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireNonAlphanumeric { get; set; } = true;
}