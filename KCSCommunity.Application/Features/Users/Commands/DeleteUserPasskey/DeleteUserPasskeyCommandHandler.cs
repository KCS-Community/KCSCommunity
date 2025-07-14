using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace KCSCommunity.Application.Features.Users.Commands.DeleteUserPasskey;

public class DeleteUserPasskeyCommandHandler : IRequestHandler<DeleteUserPasskeyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserPasskeyCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteUserPasskeyCommand request, CancellationToken cancellationToken)
    {
        var passkey = await _context.PasskeyCredentials
            .FirstOrDefaultAsync(p => p.Id == request.PasskeyId && p.UserId == request.UserId, cancellationToken);
        
        if (passkey == null)
        {
            throw new NotFoundException($"Passkey '{request.PasskeyId}' for user '{request.UserId}' not found.");
        }

        _context.PasskeyCredentials.Remove(passkey);
        await _context.SaveChangesAsync(cancellationToken);
    }
}