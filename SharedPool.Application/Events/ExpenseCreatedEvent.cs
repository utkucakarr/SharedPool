namespace SharedPool.Application.Events
{
    // Event nesnelerinde sadece Consumer'ın (tüketicinin) ihtiyacı olan verileri taşıyoruz
    public record ExpenseCreatedEvent
    {
        public Guid ExpenseId { get; init; }
        public Guid GroupId { get; init; }
        public Guid PayerUserId { get; init; }
        public decimal TotalAmount { get; init; }

        // Kimin ne kadar borcu olduğu bilgisini de gönderiyoruz ki Consumer tekrar DB'ye gitmesin
        public List<ExpenseSplitEventModel> Splits { get; init; } = new();
    }

    public record ExpenseSplitEventModel(Guid UserId, decimal OwedAmount);
}
