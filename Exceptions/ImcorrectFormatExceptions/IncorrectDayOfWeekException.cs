namespace GNS.Exceptions
{
    public class IncorrectDayOfWeekException: IncorrectFormatException
    {
        public IncorrectDayOfWeekException(
            string enteredValue,
            string additionalInfo = ""
            )
            : base(
                "DayOfWeek",
                "Value can be only common day of week",
                enteredValue,
                additionalInfo)
        {
            
        }    
    }
}