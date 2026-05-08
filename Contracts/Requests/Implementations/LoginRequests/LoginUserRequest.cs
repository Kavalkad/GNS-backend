using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;


namespace GNS.Contracts.Requests
{
    public record class LoginUserRequest : IEmailRequest, IPasswordRequest
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
        
    }
}