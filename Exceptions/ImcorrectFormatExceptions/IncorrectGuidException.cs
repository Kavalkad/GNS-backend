namespace GNS.Exceptions
{
    public class IncorrectGuidException(
        string enteredValue,
        string additionalInfo = ""
            ) : IncorrectFormatException(
            "Guid",
            "Value must have Guid format",
            enteredValue,
            additionalInfo)
    {
    }
}