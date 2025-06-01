namespace FastTechFoods.SDK.Domain
{
    public class BaseEntity
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime UpdatedAt { get; protected set; } = DateTime.Now;
    }
}
