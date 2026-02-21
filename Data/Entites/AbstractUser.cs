namespace GNS.Data.Entities
{
    public abstract class AbstractUser : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public string HashedPassword { get; set; } = string.Empty;
    }
}