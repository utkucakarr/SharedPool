using SharedPool.Domain.Enums;

namespace SharedPool.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public string Description { get; private set; }
        public decimal TotalAmount { get; private set; }
        public Guid GroupId { get; private set; }
        public Guid PayerUserId { get; private set; } // Hesabı kim ödedi?
        public DateTime ExpenseDate { get; private set; }
        public SplitType SplitType { get; private set; } = SplitType.Equal; // + STRATEJİ İÇİN EKLENDİ

        // Navigation Properties
        public virtual Group Group { get; private set; }
        public virtual User PayerUser { get; private set; }
        public virtual ICollection<ExpenseSplit> ExpenseSplits { get; private set; } // Bu harcamanın kimlere nasıl bölüneceği

        public Expense(string description, decimal totalAmount, Guid groupId, Guid payerUserId, DateTime expenseDate)
        {
            Description = description;
            TotalAmount = totalAmount;
            GroupId = groupId;
            PayerUserId = payerUserId;
            ExpenseDate = expenseDate;

            ExpenseSplits = new HashSet<ExpenseSplit>();
        }

        protected Expense() { }
    }
}
