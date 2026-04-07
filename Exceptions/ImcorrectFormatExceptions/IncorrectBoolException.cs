namespace GNS.Exceptions
{
    public class IncorrectBoolException : IncorrectValueException
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