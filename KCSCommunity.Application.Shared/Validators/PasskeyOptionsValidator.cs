using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Shared.Validators;


public class PasskeyOptionsValidator : IPasskeyOptionsValidator
{
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public PasskeyOptionsValidator(IStringLocalizer<SharedValidationMessages> localizer)
    {
        _localizer = localizer;
    }

    public void Validate(Fido2SessionData? options)
    {
        if (options?.CreateOptions == null)
        {
            throw new InvalidOperationException(_localizer["PasskeyOptionsNotFound"]);
        }
    }
}