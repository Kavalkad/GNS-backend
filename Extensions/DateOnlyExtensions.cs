using GNS.Enums;

namespace GNS.Extensions
{
    public static class DateOnluExtensions
    {
        public static CustomDayOfWeek ParseToCustomDayOfWeek(this DateOnly date)
        {
            var dayOfWeek = Enum.GetName(date.DayOfWeek)
                ?? throw new Exception($"Cannot get day of week of {date} wile convert to custom day of week");

            return Enum.Parse<CustomDayOfWeek>(dayOfWeek);
        }
    }
}