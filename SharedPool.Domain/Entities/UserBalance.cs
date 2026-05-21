namespace SharedPool.Domain.Entities
{
    public class UserBalance : BaseEntity
    {
        public Guid GroupId { get; private set; }
        public Guid OwedByUserId { get; private set; } // Borçlu olan kişi
        public Guid OwedToUserId { get; private set; } // Alacaklı olan kişi (Hesabı ödeyen)
        public decimal Amount { get; private set; }

        public UserBalance(Guid groupId, Guid owedByUserId, Guid owedToUserId, decimal amount)
        {
            GroupId = groupId;
            OwedByUserId = owedByUserId;
            OwedToUserId = owedToUserId;
            Amount = amount;
        }

        // Mevcut borcun üzerine ekleme yapmak için
        public void AddAmount(decimal amountToAdd)
        {
            Amount += amountToAdd;
        }

        // Borç ödendiğinde veya azaldığında
        public void DeductAmount(decimal amountToDeduct)
        {
            Amount -= amountToDeduct;
        }
    }
}
