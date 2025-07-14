using FluentValidation;

namespace KCSCommunity.Application.Features.Users.Commands.DeleteUserPasskey;

public class DeleteUserPasskeyCommandValidator : AbstractValidator<DeleteUserPasskeyCommand>
{
    public DeleteUserPasskeyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PasskeyId).NotEmpty();
    }
}