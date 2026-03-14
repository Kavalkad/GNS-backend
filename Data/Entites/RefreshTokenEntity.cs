namespace GNS.Data.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {
        public Guid Token { get; set; } 
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked => RevokedAt is not null;
        
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}