using MassTransit;
using SharedPool.Application.Events;
using SharedPool.Domain.Entities;
using SharedPool.Domain.Interfaces;

namespace SharedPool.Application.Consumers
{
    public class ExpenseCreatedEventConsumer : IConsumer<ExpenseCreatedEvent>
    {
        private readonly IGenericRepository<UserBalance> _balanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseCreatedEventConsumer(
            IGenericRepository<UserBalance> balanceRepository,
            IUnitOfWork unitOfWork)
        {
            _balanceRepository = balanceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<ExpenseCreatedEvent> context)
        {
            var message = context.Message;

            // Mesajın içindeki borç dağılımında dönüyoruz
            foreach (var split in message.Splits)
            {
                // Kişi kendi ödediği hesaptan kendine borçlanmaz, onu atlıyoruz
                if (split.UserId == message.PayerUserId)
                    continue;

                // Veritabanında bu iki kişi arasında daha önceden oluşmuş bir borç kaydı var mı diye bakıyoruz
                var existingBalance = (await _balanceRepository.GetAsync(b =>
                    b.GroupId == message.GroupId &&
                    b.OwedByUserId == split.UserId &&
                    b.OwedToUserId == message.PayerUserId)).FirstOrDefault();

                if (existingBalance != null)
                {
                    // Varsa mevcut borcun üzerine ekle
                    existingBalance.AddAmount(split.OwedAmount);
                    _balanceRepository.Update(existingBalance);
                }
                else
                {
                    // Yoksa sıfırdan yeni bir borç kaydı oluştur
                    var newBalance = new UserBalance(
                        message.GroupId,
                        split.UserId,
                        message.PayerUserId,
                        split.OwedAmount);

                    await _balanceRepository.AddAsync(newBalance);
                }
            }

            // Değişiklikleri kaydet (Bu işlem ana API isteğinden bağımsız, arka planda çalışır!)
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
