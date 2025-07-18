using FluentValidation;
using KCSCommunity.Abstractions.Models.Configuration;
using KCSCommunity.Application.Shared.Validators;

namespace KCSCommunity.Application.Features.Users.Commands.ActivateUser;
public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
{
    public ActivateUserCommandValidator(PasswordPolicySettings passwordPolicy)
    {
        RuleFor(x => x.Passcode).NotEmpty().Length(8);
        //RuleFor(x => x.UserName).NotEmpty();
        //RuleFor(x => x.Nickname).MaximumLength(50);
        //留给信息更新接口
        RuleFor(x => x.Password).ApplyPasswordPolicy(passwordPolicy);
    }
}