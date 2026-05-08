namespace GNS.Exceptions
{
    public class IncorrectGuidException: IncorrectFormatException
    {
       
        public IncorrectGuidException(
            string enteredValue,
            string additionalInfo = ""
            )
            : base(
                "Guid",
                "Value must have Guid format",
                enteredValue,
                additionalInfo)
        {
            
        }    
    }
}