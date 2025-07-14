using Fido2NetLib;
using Fido2NetLib.Objects;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Interfaces.Services;
using KCSCommunity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KCSCommunity.Infrastructure.Services;

public class Fido2PasskeyService : IPasskeyService
{
    private readonly IFido2 _fido2;
    private readonly IApplicationDbContext _context;

    public Fido2PasskeyService(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _fido2 = new Fido2(new Fido2Configuration
        {
            ServerDomain = configuration["Fido2:ServerDomain"],
            ServerName = configuration["Fido2:ServerName"],
            Origins = configuration.GetSection("Fido2:Origins").Get<HashSet<string>>(),
            TimestampDriftTolerance = configuration.GetValue<int?>("Fido2:TimestampDriftTolerance") ?? 300000
        });
    }

    public CredentialCreateOptions GenerateRegistrationOptions(ApplicationUser user, string deviceName, List<PasskeyCredential> existingCredentials)
    {
        var fidoUser = new Fido2User { Id = user.Id.ToByteArray(), Name = user.UserName!, DisplayName = user.RealName };
        var existingDescriptors = existingCredentials.Select(c => c.GetDescriptor()).ToList();
        
        var authenticatorSelection = new AuthenticatorSelection
        {
            UserVerification = UserVerificationRequirement.Preferred,
            RequireResidentKey = true
        };

        return _fido2.RequestNewCredential(fidoUser, existingDescriptors, authenticatorSelection, AttestationConveyancePreference.None);
    }

    public async Task<PasskeyCredential> CompleteRegistrationAsync(AuthenticatorAttestationRawResponse attestationResponse, CredentialCreateOptions originalOptions, Guid userId, string deviceName)
    {
        var callback = new IsCredentialIdUniqueToUserAsyncDelegate(async (args, cancellationToken) =>
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
            if (!userExists) return false;

            var credentialId = Convert.ToBase64String(args.CredentialId);
            return !await _context.PasskeyCredentials.AnyAsync(c => c.Id == credentialId, cancellationToken);
        });

        // CORRECTED: Fido2.MakeNewCredentialAsync returns Fido2.CredentialMakeResult
        var result = await _fido2.MakeNewCredentialAsync(attestationResponse, originalOptions, callback);
        
        // CORRECTED: Check for success
        if (result.Result is null)
        {
            throw new Exception("Passkey registration failed: " + result.ErrorMessage);
        }

        // CORRECTED: Mapping from the result object
        return new PasskeyCredential
        {
            Id = Convert.ToBase64String(result.Result.CredentialId),
            UserId = userId,
            DeviceName = deviceName,
            PublicKey = result.Result.PublicKey,
            UserHandle = result.Result.User.Id,
            SignatureCounter = result.Result.Counter, // This is the correct property name
            CredType = result.Result.CredType,
            RegDate = DateTime.UtcNow,
            AaGuid = result.Result.Aaguid
        };
    }

    
    public AssertionOptions GenerateLoginOptions(string? userName = null)
    {
        return _fido2.GetAssertionOptions(
            null,
            UserVerificationRequirement.Preferred
        );
    }
    
    public async Task<Guid> CompleteLoginAsync(AuthenticatorAssertionRawResponse clientResponse, AssertionOptions originalOptions)
    {
        // 1. Get the credential ID from the client's response.
        var credentialIdBase64 = Convert.ToBase64String(clientResponse.Id);
        var credential = await _context.PasskeyCredentials
                                     .AsNoTracking() // Use AsNoTracking because we will update the counter manually later.
                                     .FirstOrDefaultAsync(c => c.Id == credentialIdBase64);
        
        if (credential == null)
        {
            throw new Fido2VerificationException("Unknown credential. It may have been revoked or never registered.");
        }

        // 2. This is the callback that Fido2NetLib will use to get the stored public key and signature counter.
        // It provides the library with the necessary data to perform the cryptographic verification.
        var callback = new IsUserHandleOwnerOfCredentialIdAsync(async (args, cancellationToken) =>
        {
            // We've already fetched the credential, so we can just return it.
            // This confirms that the credential ID sent by the browser is one we know about.
            return true; 
        });

        // 3. Perform the assertion verification.
        var result = await _fido2.MakeAssertionAsync(clientResponse, originalOptions, credential.PublicKey, credential.SignatureCounter, callback);
        
        if (result.Status != "ok")
        {
            throw new Fido2VerificationException("Passkey login failed: " + result.ErrorMessage);
        }

        // 4. If verification is successful, update the signature counter in the database to prevent replay attacks.
        var storedCredential = await _context.PasskeyCredentials.FindAsync(credentialIdBase64);
        if (storedCredential != null)
        {
            storedCredential.SignatureCounter = (uint)result.Counter;
            // The command handler will call SaveChangesAsync.
        }
        else
        {
            // This should be an impossible scenario if the first check passed, but as a safeguard.
            throw new Exception("Failed to find credential for counter update after successful verification.");
        }
        
        // 5. Return the User ID associated with the credential.
        return credential.UserId;
    }
}