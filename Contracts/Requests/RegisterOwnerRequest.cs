using System.ComponentModel.DataAnnotations;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class RegisterOwnerRequest : RegisterUserRequest
    {
        [Required] public string SuperSecretWord { get; set; } = string.Empty;
        [Required] public string TaxIdentificationNumber { get; set; } = string.Empty;
        
    }
}