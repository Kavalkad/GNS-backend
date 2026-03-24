using GNS.Enums;

namespace GNS.Data.Entities
{
    public class CyberClubWorkingHoursEntity : BaseEntity
    {
        public Guid WorkingHoursId { get; set; }
        public Guid CyberClubId { get; set; } 
    }
}