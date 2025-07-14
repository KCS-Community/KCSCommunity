using KCSCommunity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KCSCommunity.Domain.Entities;

/// <summary>
/// 拓展内置的IdentityUser<Guid>，加入新的字段
/// 这是一个Aggregate Root聚合根
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    [MaxLength(50)]
    public string RealName { get; private set; } = string.Empty;

    [MaxLength(100)]
    public string? EnglishName { get; private set; }

    public Gender Gender { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    [MaxLength(50)]
    public string? Grade { get; private set; }

    [MaxLength(50)]
    public string? House { get; private set; }

    [Required]
    public UserRoleType RoleType { get; private set; }

    [MaxLength(100)]
    public string? StaffTitle { get; private set; }

    [MaxLength(50)]
    public string? Nickname { get; private set; }

    public string? AvatarUrl { get; private set; }

    /// <summary>
    /// Indicates if the user has completed the activation process.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// For administrative actions like temporary bans.
    /// </summary>
    public DateTime? TimeoutEndDate { get; private set; }
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; private set; }
    
    public virtual ICollection<PasskeyCredential> PasskeyCredentials { get; private set; } = new List<PasskeyCredential>();

    // Private constructor for EF Core and Identity hydratation.
    private ApplicationUser() { }

    /// <summary>
    /// Factory method for creating a new, unactivated user. This is the only public way to create a user instance.
    /// </summary>
    public static ApplicationUser CreateNewUser(
        string userName,
        //string email,
        string realName,
        string? englishName,
        //Gender gender,
        //DateTime dateOfBirth,
        UserRoleType roleType,
        string? grade,
        string? house,
        string? staffTitle)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            //Email = email,
            SecurityStamp = Guid.NewGuid().ToString("D"), // Important for security operations
            RealName = realName,
            EnglishName = englishName,
            //Gender = gender,
            //DateOfBirth = dateOfBirth,
            RoleType = roleType,
            IsActive = false, // Must be activated
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (roleType == UserRoleType.Student)
        {
            user.Grade = grade;
            user.House = house;
        }
        else if (roleType == UserRoleType.Staff)
        {
            user.StaffTitle = staffTitle;
        }

        return user;
    }

    /// <summary>
    /// Domain method to activate a user's account, enforcing business rules.
    /// </summary>
    public void ActivateAccount(/*string? nickname, string? avatarUrl*/)
    {
        if (IsActive)
        {
            throw new InvalidOperationException("Account is already active.");
        }

        //Nickname = nickname;
        //AvatarUrl = avatarUrl;
        IsActive = true;
        EmailConfirmed = true; // Activating implies the user has control of the flow.
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Domain method for an administrator to update a user's core information.
    /// </summary>
    public void UpdateInformation(string realName, string? englishName, Gender gender, DateTime dateOfBirth, UserRoleType roleType, string? grade, string? house, string? staffTitle)
    {
        RealName = realName;
        EnglishName = englishName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        RoleType = roleType;
        Grade = (roleType == UserRoleType.Student) ? grade : null;
        House = (roleType == UserRoleType.Student) ? house : null;
        StaffTitle = (roleType == UserRoleType.Staff) ? staffTitle : null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Domain method to set a timeout period for a user.
    /// </summary>
    public void SetTimeout(DateTime endDate)
    {
        if (endDate <= DateTime.UtcNow)
        {
            throw new ArgumentException("Timeout end date must be in the future.", nameof(endDate));
        }
        TimeoutEndDate = endDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Domain method to clear a user's timeout.
    /// </summary>
    public void ClearTimeout()
    {
        TimeoutEndDate = null;
        UpdatedAt = DateTime.UtcNow;
    }
}