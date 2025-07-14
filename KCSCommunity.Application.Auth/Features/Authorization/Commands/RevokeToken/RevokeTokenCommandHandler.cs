using KCSCommunity.Abstractions.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KCSCommunity.Application.Auth.Features.Authorization.Commands.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IApplicationDbContext _context;

    public RevokeTokenCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => 
                    rt.UserId == request.UserId &&
                    rt.RevokedAt == null && //!IsRevoked
                    rt.UsedAt == null && //!IsUsed
                    rt.ExpiresAt > DateTime.UtcNow //!IsExpired
            )
            .ToListAsync(cancellationToken);

        if (tokens.Any())
        {
            tokens.ForEach(t => t.RevokedAt = DateTime.UtcNow);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}