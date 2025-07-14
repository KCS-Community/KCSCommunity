using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.ExpiredAccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}