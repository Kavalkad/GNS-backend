namespace GNS.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public override string Message { get; } = string.Empty;
        public int StatusCode { get; set; }
        public EntityNotFoundException(string entityName, string additionalInfo = "")
        {
            Message = entityName + " not found." + additionalInfo;
        }    
    }
}