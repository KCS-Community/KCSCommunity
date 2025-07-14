using Fido2NetLib;

namespace KCSCommunity.Abstractions.Models.Dtos;

public class Fido2SessionData
{
    public CredentialCreateOptions? CreateOptions { get; set; }
    //public string? RegistrationUserName { get; set; }
    //public string? RegistrationAvatarUrl { get; set; }
    //public string? RegistrationNickname { get; set; }

    public string? RegistrationDeviceName { get; set; }
    public string? RegistrationPasscode { get; set; }

    public AssertionOptions? AssertionOptions { get; set; }
}