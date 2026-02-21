namespace GNS.Data.Entities
{
    public class OwnerEntity : UserEntity
    {
        public string HashedSecretWord { get; set; } = string.Empty;
        public ICollection<CyberClubEntity> CyberClubs { get; set; } = new List<CyberClubEntity>();
    }
}