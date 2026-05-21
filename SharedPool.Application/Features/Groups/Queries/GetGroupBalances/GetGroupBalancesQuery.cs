using MediatR;
using SharedPool.Application.DTOs;

namespace SharedPool.Application.Features.Groups.Queries.GetGroupBalances
{
    public record GetGroupBalancesQuery(Guid GroupId) : IRequest<List<GroupBalanceDto>>;
}