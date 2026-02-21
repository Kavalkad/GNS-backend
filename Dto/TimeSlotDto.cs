namespace GNS.Dto
{
    public class TimeSlotDto
    {
        
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public TimeSlotDto(TimeOnly startTime, TimeOnly endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}