using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Responses
{
    public record class LoginResponse
    {
        [Required] public string AccessToken { get; set; } = string.Empty;
        [Required] public string RefreshToken { get; set; } = string.Empty;
        [Required] public string Role { get; set; } = string.Empty;
    }
}