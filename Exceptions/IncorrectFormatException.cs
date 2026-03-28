namespace GNS.Exceptions
{
    public class IncorrectFormatException: Exception
    {
        public override string Message { get; } = string.Empty;
        public int StatusCode { get; set; }
        public IncorrectFormatException(
            string enteredValueType,
            string constraintsMessage,
            string additionalInfo = "")
        {
            Message = enteredValueType + " has incorrect format. " + constraintsMessage + " " + additionalInfo;
        }    
    }
}