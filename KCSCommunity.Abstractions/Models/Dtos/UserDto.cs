namespace KCSCommunity.Abstractions.Models.Dtos;

public record class UserDto
{
    public UserDto() { }

    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string RealName { get; init; } = string.Empty;
    public string? Nickname { get; init; }
    public string? AvatarUrl { get; init; }
    public bool IsActive { get; init; }
    public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
}