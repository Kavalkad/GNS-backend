using GNS.Enums;
using GNS.Interfaces;

namespace GNS.Data.Entities
{
    public class EmployeeEntity : UserEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string HashedSecretWord { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public decimal Bonus { get; set; } = 0;
        public decimal Penalty { get; set; } = 0;



        public Guid CyberClubId { get; set; }
        public CyberClubEntity? CyberClub { get; set; }


    }
}