using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateEmployeeNameRequest : IEmployeeRequest, INameRequest
    {
        [Required] public Guid EmployeeId { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
    }
}