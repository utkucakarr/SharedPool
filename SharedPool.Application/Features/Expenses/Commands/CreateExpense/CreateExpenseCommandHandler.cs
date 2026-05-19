using MediatR;
using SharedPool.Domain.Entities;
using SharedPool.Domain.Enums;
using SharedPool.Domain.Exceptions;
using SharedPool.Domain.Interfaces;
using SharedPool.Domain.Strategies;

namespace SharedPool.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Guid>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IGenericRepository<ExpenseSplit> _expenseSplitRepository;
        private readonly IGenericRepository<UserGroup> _userGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExpenseCommandHandler(
            IGenericRepository<Expense> expenseRepository,
            IGenericRepository<ExpenseSplit> expenseSplithRepository,
            IGenericRepository<UserGroup> userGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _expenseRepository = expenseRepository;
            _expenseSplitRepository = expenseSplithRepository;
            _userGroupRepository = userGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            // 1. Grubu ve gruptaki üyeleri getir
            var groupMemberships = await _userGroupRepository.GetAsync(ug => ug.GroupId == request.GroupId);
            if (!groupMemberships.Any())
            {
                throw new BusinessException("Belirtilen grup bulunamadı veya grupta hiç üye yok.");
            }

            // Gruptaki tüm kullanıcı ID'lerini bir listeye alalım
            var validMemberIds = groupMemberships.Select(ug => ug.UserId).ToList();

            // 2. Hesabı ödeyen kişi bu grupta mı?
            if (!validMemberIds.Contains(request.PayerUserId))
            {
                throw new BusinessException("Hesabı ödeyen kişi bu grubun bir üyesi değil.");
            }

            // 3. Bölüşüm listesindeki herkes bu grupta mı?
            var splitUserIds = request.SplitDetails.Select(s => s.UserId).Distinct().ToList();
            var invalidUsers = splitUserIds.Except(validMemberIds).ToList();

            if (invalidUsers.Any())
            {
                throw new BusinessException("Bölüşüm listesindeki bazı kullanıcılar bu grubun üyesi değil! Sadece grup üyelerine borç yazılabilir.");
            }

            // 1. Stratejiyi Seç (Factory Pattern'in basit bir switch expression hali)
            ISplitStrategy strategy = request.SplitType switch
            {
                SplitType.Equal => new EqualSplitStrategy(),
                SplitType.Percentage => new PercentageSplitStrategy(),
                SplitType.ExactAmount => new ExactAmountSplitStrategy(),
                _ => throw new BusinessException("Geçersiz bölüşüm tipi!")
            };

            // 2. Hesaplamayı Yap (Bütün karmaşık matematik Domain katmanında çözülüyor)
            var calculatedSplits = strategy.Calculate(request.TotalAmount, request.SplitDetails);

            // 3. Harcama (Expense) Kaydını Oluştur
            var expense = new Expense(
                request.Description,
                request.TotalAmount,
                request.GroupId,
                request.PayerUserId,
                request.ExpenseDate);

            await _expenseRepository.AddAsync(expense);

            // 4. Bölüşüm (ExpenseSplit) Kayıtlarını Oluştur
            foreach (var split in calculatedSplits)
            {
                var expenseSplit = new ExpenseSplit(expense.Id, split.UserId, split.CalculatedAmount);
                await _expenseSplitRepository.AddAsync(expenseSplit);
            }

            // 5. Veritabanına Kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return expense.Id;
        }
    }
}
