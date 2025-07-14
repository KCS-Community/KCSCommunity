using KCSCommunity.Abstractions.Interfaces.Validators;
using KCSCommunity.Application.Shared.Exceptions;
using KCSCommunity.Application.Shared.Resources;
using KCSCommunity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Shared.Validators;

public class UserStatusValidator : IUserStatusValidator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;

    public UserStatusValidator(UserManager<ApplicationUser> userManager, IStringLocalizer<SharedValidationMessages> localizer)
    {
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task ValidateAsync(ApplicationUser user)
    {
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var message = lockoutEnd.HasValue 
                ? _localizer["LoginLockedOut", lockoutEnd.Value.ToLocalTime().ToString("F")]
                : _localizer["LoginLockedOutPermanently"];
            throw new UserValidationException(message);
        }

        if (!user.IsActive)
        {
            var message = _localizer["LoginAccountNotActive"];
            throw new UserValidationException(message);
        }

        if (user.TimeoutEndDate.HasValue && user.TimeoutEndDate.Value > DateTime.UtcNow)
        {
            var message = _localizer["LoginTimedOut",user.TimeoutEndDate.Value.ToLocalTime().ToString("F")];
            throw new UserValidationException(message);
        }
    }
}