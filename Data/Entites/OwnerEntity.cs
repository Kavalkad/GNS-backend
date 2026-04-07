using GNS.Enums;

namespace GNS.Data.Entities
{
    public class OwnerEntity(
        string email,
        string hashedPassword,
        string userName,
        string hashedSuperSecretWord,
        string taxIdentificationNumber,
        Role role,
        Guid bloomBytesId
            ) : UserEntity(
                email: email,
                hashedPassword: hashedPassword,
                userName: userName,
                bloomBytesId: bloomBytesId,
                role: role
                )
    {
        public string HashedSuperSecretWord { get; set; } = hashedSuperSecretWord;
        public string TaxIdentificationNumber { get; set; } = taxIdentificationNumber;
        
        public ICollection<CyberClubEntity> CyberClubs { get; set; } = [];
    }
}