using KCSCommunity.Domain.Entities;

namespace KCSCommunity.Abstractions.Interfaces.Validators;

public interface IUserStatusValidator
{
    Task ValidateAsync(ApplicationUser user);
}