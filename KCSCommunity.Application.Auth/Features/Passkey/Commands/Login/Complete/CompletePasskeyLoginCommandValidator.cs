using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Complete;

public class CompletePasskeyLoginCommandValidator : AbstractValidator<CompletePasskeyLoginCommand>
{
    public CompletePasskeyLoginCommandValidator()
    {
        RuleFor(x => x.ClientResponse).NotEmpty();
    }
}