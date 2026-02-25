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
        public BloomBytesEntity BlomBytes { get; set; } = null!;
    }
}