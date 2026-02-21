using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class RegisterUserRequest
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
        [Required] public string UserName { get; set; } = null!;
    }
}