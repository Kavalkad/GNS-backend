namespace GNS.Exceptions
{
    public class IncorrectValueException: Exception
    {
        public override string Message { get; } = string.Empty;
        public int StatusCode { get; set; }
        public IncorrectValueException(
            string enteredValueType,
            string constraintsMessage,
            string enteredValue,
            string additionalInfo )
        {
            Message = $"{enteredValueType} has incorrect format. {constraintsMessage} Value was: {enteredValue}. {additionalInfo}";
        }    
    }
}