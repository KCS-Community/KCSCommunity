using FluentValidation;

namespace KCSCommunity.Application.Features.Roles.Commands.UpdateUsersInRole;


public class UpdateUsersInRoleCommandValidator : AbstractValidator<UpdateUsersInRoleCommand>
{
    public UpdateUsersInRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.UserIds).NotNull().NotEmpty();
    }
}