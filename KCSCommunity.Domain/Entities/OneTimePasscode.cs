using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace KCSCommunity.Domain.Entities;

public class OneTimePasscode
{
    [Key]
    public int Id { get; private set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; private set; } = string.Empty;

    [Required]
    public Guid UserId { get; private set; }
    public virtual ApplicationUser User { get; private set; } = null!;

    [Required]
    public DateTime ExpiryDate { get; private set; }

    public bool IsUsed { get; private set; }
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Private constructor for EF Core.
    private OneTimePasscode() { }

    /// <summary>
    /// Factory method for creating a new one-time passcode.
    /// </summary>
    public static OneTimePasscode Create(Guid userId, int lifespanMinutes = 1440) // Default: 24 hours
    {
        return new OneTimePasscode
        {
            UserId = userId,
            Code = GenerateUniqueCode(),
            ExpiryDate = DateTime.UtcNow.AddMinutes(lifespanMinutes),
            IsUsed = false
        };
    }

    /// <summary>
    /// Marks the passcode as used, enforcing business rules (not expired, not already used).
    /// </summary>
    public void MarkAsUsed()
    {
        if (IsUsed)
        {
            throw new InvalidOperationException("This passcode has already been used.");
        }
        if (DateTime.UtcNow > ExpiryDate)
        {
            throw new InvalidOperationException("This passcode has expired.");
        }
        IsUsed = true;
    }

    /// <summary>
    /// Generates a cryptographically-strong, human-readable code.
    /// </summary>
    private static string GenerateUniqueCode()
    {
        // A simple, human-readable code generator. Omits 'O', '0', 'I', '1' to avoid confusion.
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[RandomNumberGenerator.GetInt32(s.Length)]).ToArray());
    }
}