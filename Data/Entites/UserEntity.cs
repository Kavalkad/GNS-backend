using GNS.Enums;
using GNS.Interfaces;


namespace GNS.Data.Entities
{
    public class UserEntity : AbstractUser, IClaimsGeneratable
    {
        public string UserName { get; set; } = string.Empty;
        public Role Role { get; set; }

        public ICollection<OrderEntity> Orders { get; set; } = [];

        public Guid BloomBytesId { get; set; }
        public BloomBytesEntity BloomBytes { get; set; } = null!;

        public RefreshTokenEntity? RefreshToken { get; set; } 
        public UserEntity(
            string email,
            string hashedPassword,
            string userName,
            Guid bloomBytesId,
            Role role = Role.User)
        {
            Email = email;
            HashedPassword = hashedPassword;
            UserName = userName;
            Role = role;
            BloomBytesId = bloomBytesId;
        }
    }
}