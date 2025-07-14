using KCSCommunity.Abstractions.Models.Dtos;

namespace KCSCommunity.Abstractions.Interfaces.Validators;

public interface IPasskeyOptionsValidator
{
    void Validate(Fido2SessionData? user);
}