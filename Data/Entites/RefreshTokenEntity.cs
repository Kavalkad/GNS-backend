namespace GNS.Data.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {
        public Guid Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        
    }
}