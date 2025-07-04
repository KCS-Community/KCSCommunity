using KCSCommunity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KCSCommunity.Infrastructure.Persistence.Configurations;

public class OneTimePasscodeConfiguration : IEntityTypeConfiguration<OneTimePasscode>
{
    public void Configure(EntityTypeBuilder<OneTimePasscode> builder)
    {
        builder.HasIndex(x => x.Code).IsUnique();
        
        //indexing UserId
        builder.HasIndex(x => x.UserId);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("now() at time zone 'utc'");

        builder.HasOne(p => p.User)
            .WithMany() //A user can have multiple passcodes over their lifetime
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}