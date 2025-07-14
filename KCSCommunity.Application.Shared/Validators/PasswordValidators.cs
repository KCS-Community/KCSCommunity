using FluentValidation;
using KCSCommunity.Abstractions.Models.Configuration;

namespace KCSCommunity.Application.Shared.Validators;

public static class PasswordValidators
{
    public static IRuleBuilderOptions<T, string> ApplyPasswordPolicy<T>(
        this IRuleBuilder<T, string> ruleBuilder, 
        PasswordPolicySettings policy)
    {
        var rule = ruleBuilder
            .NotEmpty()
            .MinimumLength(policy.RequiredLength);

        if (policy.RequireUppercase)
        {
            rule = rule.Matches("[A-Z]").WithMessage("PasswordUppercaseValidator");
        }
        
        if (policy.RequireLowercase)
        {
            rule = rule.Matches("[a-z]").WithMessage("PasswordLowercaseValidator");
        }

        if (policy.RequireDigit)
        {
            rule = rule.Matches("[0-9]").WithMessage("PasswordDigitValidator");
        }
        
        if (policy.RequireNonAlphanumeric)
        {
            rule = rule.Matches("[^a-zA-Z0-9]").WithMessage("PasswordNonAlphanumericValidator");
        }
        return rule;
    }
}