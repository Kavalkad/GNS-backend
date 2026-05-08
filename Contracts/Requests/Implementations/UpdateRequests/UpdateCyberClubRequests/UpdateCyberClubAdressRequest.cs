using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubAddressRequest : ICyberClubRequest, IAddressRequest
    {
        [Required] public Guid CyberClubId { get; set; }
        [Required] public string Address { get; set; } = string.Empty;

    }
}