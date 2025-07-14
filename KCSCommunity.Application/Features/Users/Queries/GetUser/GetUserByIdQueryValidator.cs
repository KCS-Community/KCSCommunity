using FluentValidation;

namespace KCSCommunity.Application.Features.Users.Queries.GetUser;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}