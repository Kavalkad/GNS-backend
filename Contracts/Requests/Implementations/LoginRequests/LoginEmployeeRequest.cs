using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class LoginEmployeeRequest : LoginUserRequest, ISecretWordRequest
    {
        [Required] public string SecretWord { get; set; } = string.Empty;
        
    }
}