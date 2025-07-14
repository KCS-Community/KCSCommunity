using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.RevokeToken;

public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}