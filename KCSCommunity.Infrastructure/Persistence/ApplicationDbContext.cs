using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KCSCommunity.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<OneTimePasscode> OneTimePasscodes => Set<OneTimePasscode>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //table names
        builder.Entity<ApplicationUser>(b => { b.ToTable("Users"); });
        builder.Entity<IdentityUserClaim<Guid>>(b => { b.ToTable("UserClaims"); });
        builder.Entity<IdentityUserLogin<Guid>>(b => { b.ToTable("UserLogins"); });
        builder.Entity<IdentityUserToken<Guid>>(b => { b.ToTable("UserTokens"); });
        builder.Entity<IdentityRole<Guid>>(b => { b.ToTable("Roles"); });
        builder.Entity<IdentityRoleClaim<Guid>>(b => { b.ToTable("RoleClaims"); });
        builder.Entity<IdentityUserRole<Guid>>(b => { b.ToTable("UserRoles"); });
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // This is a good place to add logic before saving, such as automatically updating
        // 'UpdatedAt' timestamps for tracked entities, but we handle that in the domain methods for now.
        return base.SaveChangesAsync(cancellationToken);
    }
}