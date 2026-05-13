namespace SharedPool.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; } // Şifreyi açık metin tutmuyoruz.

        // Navigation Properties (İlişkiler)
        // Navigation Properties (İlişkiler)
        public virtual ICollection<UserGroup> UserGroups { get; private set; }
        public virtual ICollection<Expense> PaidExpenses { get; private set; } // Bu kullanıcının ödediği masraflar
        public virtual ICollection<ExpenseSplit> ExpenseSplits { get; private set; } // Bu kullanıcının dahil olduğu masraf bölüşümleri

        // DDD Yaklaşımı: Dışarıdan sadece yapıcı metot (constructor) veya özel metotlarla veri atanabilir.
        public User(string firstName, string lastName, string email, string passwordHash)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;

            UserGroups = new HashSet<UserGroup>();
            PaidExpenses = new HashSet<Expense>();
            ExpenseSplits = new HashSet<ExpenseSplit>();
        }

        // Boş Constructor (EF Core için)
        protected User()
        {
            
        }
    }
}
