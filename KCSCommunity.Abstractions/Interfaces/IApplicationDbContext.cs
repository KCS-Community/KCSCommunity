using KCSCommunity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace KCSCommunity.Abstractions.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<OneTimePasscode> OneTimePasscodes { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<PasskeyCredential> PasskeyCredentials { get; }
    
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}