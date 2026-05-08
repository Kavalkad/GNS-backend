using System.ComponentModel.DataAnnotations;
using GNS.Dto;

namespace GNS.Contracts.Responses
{
    public class LoginOwnerResponse : LoginUserResponse
    {
        [Required] public string TaxIdentificationNumber { get; set; } = string.Empty;

    }
}