using GNS.Enums;
using GNS.Exceptions;

namespace GNS.Extensions
{
    public static class DateTimeExtensions
    {
        public static CustomDayOfWeek ParseToCustomDayOfWeek(this DateTime date)
        {
            var dayOfWeek = Enum.GetName(date.DayOfWeek);
                
            return Enum.Parse<CustomDayOfWeek>(dayOfWeek);
        }
    }
}