using System.ComponentModel.DataAnnotations;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateEmployeeRequest 
    {
        [Required] public string EmployeeId { get; set; } = string.Empty;
    }
}