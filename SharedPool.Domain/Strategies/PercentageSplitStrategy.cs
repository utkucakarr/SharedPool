using SharedPool.Domain.Exceptions;
using SharedPool.Domain.Models;

namespace SharedPool.Domain.Strategies
{
    public class PercentageSplitStrategy : ISplitStrategy
    {
        public List<(Guid UserId, decimal CalculatedAmount)> Calculate(decimal totalAmount, List<SplitDetailModel> splitDetails)
        {
            var totalPercentage = splitDetails.Sum(x => x.Value);

            // İş kuralı: Yüzdelerin toplamı tam 100 olmak zorunda
            if (totalPercentage != 100)
            {
                throw new BusinessException($"Girilen yüzdelerin toplamı 100 olmalıdır. Şu anki toplam: {totalPercentage}");
            }

            // Herkesin yüzdesine göre tutarı hesapla (Kuruşları yuvarlayarak)
            return splitDetails.Select(x =>
                (x.UserId, Math.Round(totalAmount * (x.Value / 100), 2))
            ).ToList();
        }
    }
}
