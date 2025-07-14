using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.DeleteMyPasskey;

public class DeleteMyPasskeyCommandValidator : AbstractValidator<DeleteMyPasskeyCommand>
{
    public DeleteMyPasskeyCommandValidator()
    {
        RuleFor(x => x.PasskeyId).NotEmpty();
    }
}