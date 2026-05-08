namespace GNS.Exceptions
{
    public class IncorrectRoleNameException: IncorrectFormatException
    {
        public IncorrectRoleNameException(
            string enteredRoleName,
            string additionalInfo = ""
            )
            : base(
                "RoleName",
                "Role can be only User, Admin, Manager or Owner",
                enteredRoleName,
                additionalInfo)
        {
            
        }    
    }
}