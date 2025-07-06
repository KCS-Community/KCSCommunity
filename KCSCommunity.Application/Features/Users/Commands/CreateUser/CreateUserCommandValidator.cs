using FluentValidation;
using KCSCommunity.Application.Resources;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Features.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IStringLocalizer<SharedValidationMessages> localizer)
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RealName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateTime.Now.AddYears(-5))
            .WithMessage(localizer["UserMinimumAgeValidator"]);
    }
}