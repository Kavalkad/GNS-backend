namespace GNS.Exceptions
{
    public class IncorrectBoolException : IncorrectFormatException
    {
        public IncorrectBoolException(
            string enteredValue,
            string additionalInfo = ""
            )
            : base(
                "Bool",
                "Boolean value can be only true or false",
                enteredValue,
                additionalInfo
                )
        {

        }
    }
}