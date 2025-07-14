using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KCSCommunity.Abstractions.Interfaces.Services;

namespace KCSCommunity.Infrastructure.Security.Jwt;

public class JwtService : IJwtService
{
    private readonly IAuthTokenSettings _tokenSettings;

    public JwtService(IAuthTokenSettings tokenSettings)
    {
        _tokenSettings = tokenSettings;
    }

    public string GenerateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenSettings.GetSecret());

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Prevents token replay
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()) // Ensure NameIdentifier is present
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.GetAccessTokenExpiryMinutes()),
            Issuer = _tokenSettings.GetIssuer(),
            Audience = _tokenSettings.GetAudience(),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}