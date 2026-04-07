using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class DeleteEmployeeRequest : IEmployeeRequest
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}