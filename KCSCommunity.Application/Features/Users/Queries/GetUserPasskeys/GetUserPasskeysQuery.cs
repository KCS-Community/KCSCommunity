using KCSCommunity.Abstractions.Models.Dtos;
using MediatR;
namespace KCSCommunity.Application.Features.Users.Queries.GetUserPasskeys;

public record GetUserPasskeysQuery(Guid UserId) : IRequest<IEnumerable<PasskeyDto>>;