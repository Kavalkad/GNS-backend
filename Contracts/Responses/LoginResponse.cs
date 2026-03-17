using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Responses
{
    public record class LoginResponse
    {
        [Required] public string AccessToken { get; set; } = string.Empty;
        [Required] public string RefreshToken { get; set; } = string.Empty;
    }
}