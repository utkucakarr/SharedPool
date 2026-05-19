using SharedPool.Domain.Exceptions;
using SharedPool.Domain.Models;

namespace SharedPool.Domain.Strategies
{
    public class ExactAmountSplitStrategy : ISplitStrategy
    {
        public List<(Guid UserId, decimal CalculatedAmount)> Calculate(decimal totalAmount, List<SplitDetailModel> splitDetails)
        {
            var totalSplitAmount = splitDetails.Sum(x => x.Value);

            // İş kuralı: Girilen tam tutarların toplamı faturaya eşit olmak zorunda
            if(totalSplitAmount != totalAmount)
            {
                throw new BusinessException($"Girilen tutarların toplamı ({totalSplitAmount}), toplam fatura tutarına ({totalAmount}) eşit olmalıdır.");
            }

            return splitDetails.Select(x => (x.UserId, Math.Round(x.Value, 2))).ToList();
        }
    }
}
