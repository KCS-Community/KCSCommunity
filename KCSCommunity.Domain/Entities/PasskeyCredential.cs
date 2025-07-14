using System.ComponentModel.DataAnnotations;

namespace KCSCommunity.Domain.Entities;

public class PasskeyCredential
{
    [Key]
    [MaxLength(512)]
    public required string Id { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
    
    [MaxLength(255)]
    public string? DeviceName { get; set; }

    public byte[] PublicKey { get; set; } = Array.Empty<byte>();
    public byte[] UserHandle { get; set; } = Array.Empty<byte>();
    public uint SignatureCounter { get; set; }
    public string CredType { get; set; } = string.Empty;
    public DateTime RegDate { get; set; }
    public Guid AaGuid { get; set; }
    
    public Fido2NetLib.Objects.PublicKeyCredentialDescriptor GetDescriptor()
    {
        return new Fido2NetLib.Objects.PublicKeyCredentialDescriptor
        {
            Type = Fido2NetLib.Objects.PublicKeyCredentialType.PublicKey,
            Id = Convert.FromBase64String(Id)
        };
    }
}