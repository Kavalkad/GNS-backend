using GNS.Enums;

namespace GNS.Data.Entities
{
    public class WorkingHoursEntity : BaseEntity
    {
        public CustomDayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public bool IsOpen { get; set; }

        public Guid CyberClubId { get; set; }
        public CyberClubEntity? CyberClub { get; set; }
    }
}