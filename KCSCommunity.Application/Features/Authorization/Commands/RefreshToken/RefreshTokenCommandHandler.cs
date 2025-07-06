using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Features.Authorization.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KCSCommunity.Abstractions.Models.Configuration;
using KCSCommunity.Application.Resources;
using KCSCommunity.Domain.Entities;
using Microsoft.Extensions.Localization;

namespace KCSCommunity.Application.Features.Authorization.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IStringLocalizer<SharedValidationMessages> _localizer;


    public RefreshTokenCommandHandler(IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        JwtSettings jwtSettings,
        IStringLocalizer<SharedValidationMessages> localizer)
    {
        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings;
        _localizer = localizer;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = GetPrincipalFromExpiredToken(request.ExpiredAccessToken);
        var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId) || jti == null)
            throw new SecurityTokenException(_localizer["RefreshTokenInvalidClaims"]);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var storedRefreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

            if (storedRefreshToken == null || 
                storedRefreshToken.UserId != userId || 
                !storedRefreshToken.IsActive ||
                storedRefreshToken.JwtId != jti)
            {
                //试图使用一个无效或不匹配Refresh Token，
                //出于安全考虑吊销该用户的所有Refresh Token。
                await InvalidateUserTokensAsync(userId);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                throw new SecurityTokenException(_localizer["RefreshTokenReuseAttempt"]);
            }
            
            //标记旧的Refresh Token已被使用
            storedRefreshToken.UsedAt = DateTime.UtcNow;

            var user = await _userManager.FindByIdAsync(userIdStr);
            if (user == null) throw new SecurityTokenException("RefreshTokenUserNotFound");
            
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtService.GenerateToken(user, roles);
            
            var newJti = new JwtSecurityTokenHandler().ReadJwtToken(newAccessToken).Id;

            var newRefreshToken = new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                Token = Domain.Entities.RefreshToken.GenerateTokenValue(),
                JwtId = newJti,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            };
            
            await _context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new LoginResponse(newAccessToken, newRefreshToken.Token, user.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = false // We don't care if the token is expired here.
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException(_localizer["RefreshTokenInvalidToken"]);

        return principal;
    }
    
    private async Task InvalidateUserTokensAsync(Guid userId)
    {
        var userTokens = await _context.RefreshTokens
            .Where(rt => 
                rt.UserId == userId &&
                rt.RevokedAt == null &&
                rt.UsedAt == null &&
                rt.ExpiresAt > DateTime.UtcNow
            )
            .ToListAsync();
        
        if (userTokens.Any())
        {
            userTokens.ForEach(t => t.RevokedAt = DateTime.UtcNow);
        }
    }
}