using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Responses
{
    public record class LoginUserResponse : LoginResponse
    {
        [Required] public string UserName { get; set; } = null!;
        [Required] public virtual Role Role { get; } = Role.User; 
    }
}