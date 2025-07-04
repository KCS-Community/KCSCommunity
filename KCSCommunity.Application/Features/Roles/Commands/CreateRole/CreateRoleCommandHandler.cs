using KCSCommunity.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
namespace KCSCommunity.Application.Features.Roles.Commands.CreateRole;
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    public CreateRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager) => _roleManager = roleManager;
    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleManager.RoleExistsAsync(request.RoleName))
        {
            throw new ValidationException(new[] { new ValidationFailure("RoleName", "Role with this name already exists.") });
        }
        var role = new IdentityRole<Guid>(request.RoleName) { NormalizedName = _roleManager.NormalizeKey(request.RoleName) };
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        return role.Id;
    }
}