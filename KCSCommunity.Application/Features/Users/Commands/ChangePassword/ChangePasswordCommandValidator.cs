using FluentValidation;
namespace KCSCommunity.Application.Features.Users.Commands.ChangePassword;
public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().NotEqual(x => x.OldPassword)
            .WithMessage("New password cannot be the same as the old password.");
        RuleFor(x => x.NewPassword).MinimumLength(8).Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches("[^a-zA-Z0-9]");
    }
}