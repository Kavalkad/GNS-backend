namespace GNS.Data.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; } = Guid.NewGuid();
        
    }
}