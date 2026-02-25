namespace GNS.Data.Entities
{
    public class BloomBytesEntity : BaseEntity
    {
        public byte[] EmailBytes { get; set; } = [];
        public byte[] UserNameBytes { get; set; } = [];

        
        public ICollection<UserEntity> Users { get; set; } = [];
    }
}