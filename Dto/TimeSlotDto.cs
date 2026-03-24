namespace GNS.Dto
{
    public class TimeSlotDto
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        public TimeSlotDto(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            DateTimeStart = dateTimeStart;
            DateTimeEnd = dateTimeEnd;
        }
    }
}