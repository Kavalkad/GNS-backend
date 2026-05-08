using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;
using GNS.Interfaces;


namespace GNS.Contracts.Requests
{
    public record class GiveBonusRequest : IEmployeeRequest, IBonusRequest
    {
        [Required] public Guid EmployeeId { get; set; } 
        [Required] public decimal Bonus { get; set; } 
    }
}