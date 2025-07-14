using Fido2NetLib;
using KCSCommunity.Domain.Entities;

namespace KCSCommunity.Abstractions.Interfaces.Services;

public interface IPasskeyService
{
    CredentialCreateOptions GenerateRegistrationOptions(ApplicationUser user, string deviceName, List<PasskeyCredential> existingCredentials);
    
    Task<PasskeyCredential> CompleteRegistrationAsync(AuthenticatorAttestationRawResponse attestationResponse, CredentialCreateOptions originalOptions, Guid userId, string deviceName);

    AssertionOptions GenerateLoginOptions(string? userName = null);
    Task<Guid> CompleteLoginAsync(AuthenticatorAssertionRawResponse clientResponse, AssertionOptions originalOptions);
}