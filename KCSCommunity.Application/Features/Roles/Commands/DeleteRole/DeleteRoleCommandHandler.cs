using KCSCommunity.Application.Common.Exceptions;
using KCSCommunity.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using KCSCommunity.Application.Resources;
using KCSCommunity.Domain.Entities;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Features.Roles.Commands.DeleteRole;
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public DeleteRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager,
        IStringLocalizer<SharedValidationMessages> localizer)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _localizer = localizer;
    }
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null) throw new NotFoundException("Role", request.RoleId);
        if (role.Name == RoleConstants.Owner || role.Name == RoleConstants.Administrator || role.Name == RoleConstants.User)
        {
            throw new ValidationException(new[] { new ValidationFailure("RoleId", _localizer["RoleCannotDeleteSystemRole"]) });
        }
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
        if (usersInRole.Any())
        {
            throw new ValidationException(new[] { new ValidationFailure("RoleId", _localizer["RoleCannotDeleteRoleWithUsers"]) });
        }
        await _roleManager.DeleteAsync(role);
    }
}