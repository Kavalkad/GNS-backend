namespace GNS.Dto
{
    public class WorkingHoursDto
    {
        public Guid Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public bool IsOpen { get; set; }
    }
}