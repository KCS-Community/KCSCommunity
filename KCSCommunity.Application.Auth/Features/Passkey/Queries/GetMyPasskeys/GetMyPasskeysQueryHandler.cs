using AutoMapper;
using KCSCommunity.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KCSCommunity.Abstractions.Models.Dtos;

namespace KCSCommunity.Application.Auth.Features.Passkey.Queries.GetMyPasskeys;

public class GetMyPasskeysQueryHandler : IRequestHandler<GetMyPasskeysQuery, IEnumerable<PasskeyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GetMyPasskeysQueryHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    { _context = context; _httpContextAccessor = httpContextAccessor; _mapper = mapper; }

    public async Task<IEnumerable<PasskeyDto>> Handle(GetMyPasskeysQuery request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException();
        }

        var passkeys = await _context.PasskeyCredentials
            .Where(p => p.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<PasskeyDto>>(passkeys);
    }
}