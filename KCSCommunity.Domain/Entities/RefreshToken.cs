// src/1-Domain/KCSCommunity.Domain/Entities/RefreshToken.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace KCSCommunity.Domain.Entities;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string JwtId { get; set; } = string.Empty; //关联的AccessTokenID

    [Required]
    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

    public DateTime? UsedAt { get; set; }

    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    [NotMapped]
    public bool IsRevoked => RevokedAt != null;
    [NotMapped]
    public bool IsUsed => UsedAt != null;
    [NotMapped]
    public bool IsActive => !IsRevoked && !IsUsed && !IsExpired;
    
    public static string GenerateTokenValue(int numberOfBytes = 64)
    {
        var randomNumber = new byte[numberOfBytes];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        
        return Convert.ToBase64String(randomNumber);
    }
}