using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Responses
{
    public class LoginEmployeeResponse : LoginResponse
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
  
    }
}