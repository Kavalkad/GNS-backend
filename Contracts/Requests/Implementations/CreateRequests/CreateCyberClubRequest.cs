using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class CreateCyberClubRequest : INameRequest, ICityRequest, IAddressRequest
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string City { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
    }
}