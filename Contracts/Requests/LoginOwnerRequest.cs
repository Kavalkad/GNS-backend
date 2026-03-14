using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class LoginOwnerRequest : LoginUserRequest
    {
        [Required] public string SuperSecretWord { get; set; } = null!;
    }
}