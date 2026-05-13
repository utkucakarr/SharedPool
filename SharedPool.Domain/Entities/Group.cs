namespace SharedPool.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; private set; }
        public Guid CreatedByUserId { get; private set; }

        // Navigation Properties
        public virtual User CreatedByUser { get; private set; }
        public virtual ICollection<UserGroup> UserGroups { get; private set; }
        public virtual ICollection<Expense> Expenses{ get; private set; }

        public Group(string name, Guid createdByUserId)
        {
            Name = name;
            CreatedByUserId = createdByUserId;

            UserGroups = new HashSet<UserGroup>();
            Expenses = new HashSet<Expense>();
        }

        protected Group()
        {

        }
    }
}
