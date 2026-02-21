using System.ComponentModel.DataAnnotations;
using GNS.Enums;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class RegisterEmployeeRequest : RegisterUserRequest, IPersonRequest
    {
        [Required] public string SecretWord { get; set; } = string.Empty;
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public decimal Salary { get; set; }
        [Required] public string CyberClubName { get; set; } = string.Empty;
        [Required] public string RoleName { get; set; } = string.Empty;
    }
}