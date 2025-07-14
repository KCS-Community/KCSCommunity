using KCSCommunity.Abstractions.Models.Dtos;
using MediatR;
namespace KCSCommunity.Application.Auth.Features.Passkey.Queries.GetMyPasskeys;

public record GetMyPasskeysQuery : IRequest<IEnumerable<PasskeyDto>>;