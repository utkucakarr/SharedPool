using SharedPool.Domain.Exceptions;
using SharedPool.Domain.Models;

namespace SharedPool.Domain.Strategies
{
    public class EqualSplitStrategy : ISplitStrategy
    {
        public List<(Guid UserId, decimal CalculatedAmount)> Calculate(decimal totalAmount, List<SplitDetailModel> splitDetails)
        {
            if (splitDetails == null || splitDetails.Count == 0)
            {
                throw new BusinessException("En az bir kullanıcı olmalıdır.");
            }

            var splits = new List<(Guid UserId, decimal CalculatedAmount)>();
            int userCount = splitDetails.Count;

            // Kuruş problemi yaşamamak için virgülden sonra 2 haneye yuvarlıyoruz
            decimal baseAmount = Math.Round(totalAmount / userCount, 2);

            for(int i = 0; i < userCount; i++)
            {
                splits.Add((splitDetails[i].UserId, baseAmount));
            }

            // Kuruşlarda kayıp veya fazlalık var mı kontrol et
            decimal calculatedTotal = splits.Sum(x => x.CalculatedAmount);
            decimal difference = totalAmount - calculatedTotal;

            // Eğer kuruş farkı varsa, farkı ilk kişiye yansıt (Splitwise aynen böyle çalışır)
            if (difference != 0)
            {
                var firstUser = splits[0];
                splits[0] = (firstUser.UserId, firstUser.CalculatedAmount + difference);
            }

            return splits;
        }
    }
}