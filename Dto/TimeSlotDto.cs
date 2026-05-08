namespace GNS.Dto
{
    public record class TimeSlotDto
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }

    }
}