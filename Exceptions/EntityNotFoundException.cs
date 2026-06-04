namespace GNS.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public override string Message { get; } = string.Empty;
        public EntityNotFoundException(string entityName, string enteredValue, string additionalInfo = "")
        {
            Message = $"{entityName} not found. Entered value was: {enteredValue}. {additionalInfo}";
        }    
    }
}