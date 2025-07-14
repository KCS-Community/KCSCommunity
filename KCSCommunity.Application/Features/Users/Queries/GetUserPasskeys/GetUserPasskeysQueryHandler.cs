using AutoMapper;
using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Abstractions.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace KCSCommunity.Application.Features.Users.Queries.GetUserPasskeys;

public class GetUserPasskeysQueryHandler : IRequestHandler<GetUserPasskeysQuery, IEnumerable<PasskeyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserPasskeysQueryHandler(IApplicationDbContext context, IMapper mapper)
    { _context = context; _mapper = mapper; }

    public async Task<IEnumerable<PasskeyDto>> Handle(GetUserPasskeysQuery request, CancellationToken cancellationToken)
    {
        var passkeys = await _context.PasskeyCredentials
            .Where(p => p.UserId == request.UserId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<PasskeyDto>>(passkeys);
    }
}