using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;
using GNS.Interfaces;


namespace GNS.Contracts.Requests
{
    public record class GiveBonusRequest : IEmployeeRequest, IBonusRequest
    {
        [Required] public string EmployeeId { get; set; } = string.Empty;
        [Required] public decimal Bonus { get; set; } 
    }
}