namespace SharedPool.Domain.Entities
{
    public class UserGroup : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid GroupId { get; private set; }

        // Navigation Properties
        public virtual User User { get; private set; }
        public virtual Group Group { get; private set; }

        public UserGroup(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        protected UserGroup()
        {

        }
    }
}
