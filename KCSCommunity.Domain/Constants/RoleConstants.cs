namespace KCSCommunity.Domain.Constants;

public static class RoleConstants
{
    public const string Owner = "Owner";
    public const string Administrator = "Administrator";
    public const string User = "User";

    public static readonly string[] AllRoles = { Owner, Administrator, User };
}