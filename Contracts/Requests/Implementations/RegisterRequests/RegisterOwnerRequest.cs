using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class RegisterOwnerRequest : RegisterUserRequest, ISuperSecretWordRequest, ITaxIdentificationNumberRequest
    {
        [Required] public string SuperSecretWord { get; set; } = string.Empty;
        [Required] public string TaxIdentificationNumber { get; set; } = string.Empty;
        
    }
}