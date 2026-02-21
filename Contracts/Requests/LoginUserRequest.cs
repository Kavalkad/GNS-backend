using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class LoginUserRequest
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
        
    }
}