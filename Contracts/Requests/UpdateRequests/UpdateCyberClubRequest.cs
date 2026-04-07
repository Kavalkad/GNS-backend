using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubNameRequest : ICyberClubRequest, INameRequest
    {
        [Required] public string CyberClubId { get; set; } = string.Empty;
        [Required] public string Name { get; set; } = string.Empty; 
        
    }
}