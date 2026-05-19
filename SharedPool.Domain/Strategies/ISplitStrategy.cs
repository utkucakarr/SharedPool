using SharedPool.Domain.Models;

namespace SharedPool.Domain.Strategies
{
    public interface ISplitStrategy
    {
        // Geriye Kullanıcı Id'si ve Hesaplanmış Borç Tutarını (tuple olarak) dönecek
        List<(Guid UserId, decimal CalculatedAmount)> Calculate(decimal totalAmount, List<SplitDetailModel> splitDetails);
    }
}
