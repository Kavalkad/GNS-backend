using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Responses
{
    public record class LoginOwnerResponse
    {
        [Required] public string AccessToken { get; set; } = null!;
        [Required] public string RefreshToken { get; set; } = null!;
    }
}