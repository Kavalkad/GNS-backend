namespace GNS.Exceptions
{
    public class IncorrectTimeException: IncorrectFormatException
    {
        public IncorrectTimeException(
            string timeName,
            string enteredValue,
            string additionalInfo = ""
            )
            : base(
                timeName,
                "Value must have hh:mm format",
                enteredValue,
                additionalInfo)
        {
            
        }    
    }
}