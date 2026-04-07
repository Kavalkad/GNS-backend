using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubAddressRequest : ICyberClubRequest, IAddressRequest
    {
        [Required] public string CyberClubId { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty; 
        
    }
}