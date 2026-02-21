using System.ComponentModel.DataAnnotations;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateEmployeeRequest : IPersonRequest
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;

        public string NewFirstName { get; set; } = string.Empty;
        public string NewLastName { get; set; } = string.Empty;
        public decimal? NewSalary { get; set; }
        public string NewRoleName { get; set; } = string.Empty;
        public string NewCyberClubName { get; set; } = string.Empty;
    }
}