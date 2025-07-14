using KCSCommunity.Domain.Entities;

namespace KCSCommunity.Abstractions.Interfaces.Services;


public interface IJwtService
{
    /// <summary>
    /// Generates a JWT for a given user and their roles.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <param name="roles">The roles assigned to the user.</param>
    /// <returns>A string representation of the generated JWT.</returns>
    string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
}