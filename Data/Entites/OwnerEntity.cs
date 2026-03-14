using GNS.Enums;

namespace GNS.Data.Entities
{
    public class OwnerEntity : UserEntity
    {
        public string HashedSuperSecretWord { get; set; } = string.Empty;
        public ICollection<CyberClubEntity> CyberClubs { get; set; } = [];

        public OwnerEntity(
            string email,
            string hashedPassword,
            string userName,
            string hashedSuperSecretWord,
            Role role,
            Guid bloomBytesId
            ) : base(
                    email: email,
                    hashedPassword: hashedPassword,
                    userName: userName,
                    bloomBytesId: bloomBytesId,
                    role: role
                )
        {
            HashedSuperSecretWord = hashedSuperSecretWord;
        }
    }
}