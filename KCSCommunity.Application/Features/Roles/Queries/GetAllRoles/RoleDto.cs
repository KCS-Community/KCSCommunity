namespace KCSCommunity.Application.Features.Roles.Queries.GetAllRoles;

public record class RoleDto
{
    public RoleDto() { }
    
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}