using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class RegisterEmployeeRequest 
        : RegisterUserRequest, IPersonRequest, ICyberClubRequest, ISalaryRequest, ISecretWordRequest
    {
        [Required] public string SecretWord { get; set; } = string.Empty;
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public decimal Salary { get; set; }
        [Required] public Guid CyberClubId { get; set; } 
        [Required] public string RoleName { get; set; } = string.Empty;
    }
}