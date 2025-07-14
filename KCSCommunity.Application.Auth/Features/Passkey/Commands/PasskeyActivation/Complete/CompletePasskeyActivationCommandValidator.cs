using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Complete;

public class CompletePasskeyActivationCommandValidator : AbstractValidator<CompletePasskeyActivationCommand>
{
    public CompletePasskeyActivationCommandValidator()
    {
        RuleFor(x => x.AttestationResponse).NotEmpty();
        RuleFor(x => x.AttestationResponse).NotEmpty();
    }
}