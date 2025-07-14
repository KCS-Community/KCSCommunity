using FluentValidation;
using KCSCommunity.Abstractions.Models.Configuration;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Application.Shared.Validators;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Auth.Features.Users.Commands.ChangePassword;
public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator(PasswordPolicySettings passwordPolicy, IStringLocalizer<SharedValidationMessages> localizer)
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).ApplyPasswordPolicy(passwordPolicy)
            .NotEqual(x => x.OldPassword)
            .WithMessage(localizer["PasswordsCannotBeSame"]);
    }
}