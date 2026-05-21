using MediatR;
using SharedPool.Application.DTOs;
using SharedPool.Domain.Entities;
using SharedPool.Domain.Interfaces;

namespace SharedPool.Application.Features.Groups.Queries.GetGroupBalances
{
    public class GetGroupBalancesQueryHandler : IRequestHandler<GetGroupBalancesQuery, List<GroupBalanceDto>>
    {
        private readonly IGenericRepository<UserBalance> _balanceRepository;

        public GetGroupBalancesQueryHandler(IGenericRepository<UserBalance> balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        public async Task<List<GroupBalanceDto>> Handle(GetGroupBalancesQuery request, CancellationToken cancellationToken)
        {
            // İlgili gruba ait bakiyeleri veritabanından çek
            var balances = await _balanceRepository.GetAsync(b => b.GroupId == request.GroupId && b.Amount > 0);

            // Veritabanı modelini DTO'ya dönüştür (Mapping)
            return balances.Select(b => new GroupBalanceDto(
                b.OwedByUserId,
                b.OwedToUserId,
                b.Amount
            )).ToList();
        }
    }
}
