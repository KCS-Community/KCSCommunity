using KCSCommunity.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KCSCommunity.Application.Shared.Exceptions;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.DeleteMyPasskey;

public class DeleteMyPasskeyCommandHandler : IRequestHandler<DeleteMyPasskeyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteMyPasskeyCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    { _context = context; _httpContextAccessor = httpContextAccessor; }

    public async Task Handle(DeleteMyPasskeyCommand request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException();
        }
        
        var passkey = await _context.PasskeyCredentials
            .FirstOrDefaultAsync(p => p.Id == request.PasskeyId && p.UserId == userId, cancellationToken);
        
        if (passkey == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.PasskeyCredential), request.PasskeyId);
        }

        _context.PasskeyCredentials.Remove(passkey);
        await _context.SaveChangesAsync(cancellationToken);
    }
}