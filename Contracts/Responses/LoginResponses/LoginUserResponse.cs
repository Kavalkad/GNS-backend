using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Responses
{
    public class LoginUserResponse : LoginResponse
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string UserName { get; set; } = string.Empty;
        [Required] public Role Role { get; set; }
    }
}