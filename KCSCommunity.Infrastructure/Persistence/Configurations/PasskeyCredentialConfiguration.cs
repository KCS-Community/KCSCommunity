using KCSCommunity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KCSCommunity.Infrastructure.Persistence.Configurations;

public class PasskeyCredentialConfiguration : IEntityTypeConfiguration<PasskeyCredential>
{
    public void Configure(EntityTypeBuilder<PasskeyCredential> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.UserId);
        builder.HasOne(c => c.User)
            .WithMany() //one user can have multiple passkeys
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}