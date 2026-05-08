using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class LoginOwnerRequest : LoginUserRequest, ISuperSecretWordRequest
    {
        [Required] public string SuperSecretWord { get; set; } = null!;
    }
}