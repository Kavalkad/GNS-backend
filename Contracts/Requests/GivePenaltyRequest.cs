using System.ComponentModel.DataAnnotations;
using GNS.Interfaces;


namespace GNS.Contracts.Requests
{
    public record class GivePenaltyRequest : IPersonRequest, IPenaltyRequest
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public decimal Penalty { get; set; } 
    }
}