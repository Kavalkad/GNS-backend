namespace GNS.Exceptions
{
    public class IncorrectGuidException: IncorrectValueException
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