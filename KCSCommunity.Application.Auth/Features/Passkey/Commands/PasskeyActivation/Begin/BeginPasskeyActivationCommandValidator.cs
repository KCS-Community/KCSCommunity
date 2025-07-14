using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Begin;

public class BeginPasskeyActivationCommandValidator : AbstractValidator<BeginPasskeyActivationCommand>
{
    public BeginPasskeyActivationCommandValidator()
    {
        //RuleFor(x => x.UserName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.DeviceName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Passcode).NotEmpty();
    }
}