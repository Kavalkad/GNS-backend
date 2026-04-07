using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateEmployeeRequest  : IEmployeeRequest
    {
        [Required] public string EmployeeId { get; set; } = string.Empty;
    }
}