using GNS.Enums;

namespace GNS.Extensions
{
    public static class DateTimeExtensions
    {
        public static CustomDayOfWeek ParseToCustomDayOfWeek(this DateTime date)
        {
            var dayOfWeek = Enum.GetName(date.DayOfWeek)
                ?? throw new Exception($"Cannot get day of week of {date.Date} wile convert to custom day of week");

            return Enum.Parse<CustomDayOfWeek>(dayOfWeek);
        }
    }
}