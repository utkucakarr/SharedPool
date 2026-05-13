namespace SharedPool.Domain.Entities
{
    public class ExpenseSplit : BaseEntity
    {
        public Guid ExpenseId { get; private set; }
        public Guid OwedByUserId { get; private set; } // Borcu ödemesi gereken (borçlu) kişi
        public decimal OwedAmount { get; private set; } // Ne kadar borcu var?

        // Navigation Properties
        public virtual Expense Expense { get; private set; }
        public virtual User OwedByUser { get; private set; }

        public ExpenseSplit(Guid expenseId, Guid owedByUserId, decimal owedAmount)
        {
            ExpenseId = expenseId;
            OwedByUserId = owedByUserId;
            OwedAmount = owedAmount;
        }

        protected ExpenseSplit() { }
    }
}
