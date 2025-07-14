namespace KCSCommunity.Abstractions.Models.Dtos;

public record class RoleDto
{
    public RoleDto() { }
    
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}