using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;


namespace GNS.Contracts.Requests
{
    public record class GivePenaltyRequest :  IEmployeeRequest, IPenaltyRequest
    {
        [Required] public Guid EmployeeId { get; set; } 
        [Required] public decimal Penalty { get; set; } 
    }
}