using FluentValidation;

namespace KCSCommunity.Application.Features.Users.Queries.GetUserPasskeys;


public class GetUserPasskeysQueryValidator : AbstractValidator<GetUserPasskeysQuery>
{
    public GetUserPasskeysQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}