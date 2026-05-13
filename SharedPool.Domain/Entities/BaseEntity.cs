namespace SharedPool.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime? UpdatedDate { get; protected set; }
        public DateTime? DeletedDate { get; protected set; }
        public bool IsDeleted { get; protected set; } // Soft delete için

        // Entity Framework Core'un reflection ile çalışabilmesi için parametresiz constructor gereklidir.
        // Protected yapıyoruz ki dışarıdan anlamsız boş bir BaseEntity yaratılmasın.
        protected BaseEntity()
        {
            Id = Guid.NewGuid(); // Güvenlik ve dağıtık sistemler için int yerine Guid tercih ediyoruz.
            CreatedDate = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
