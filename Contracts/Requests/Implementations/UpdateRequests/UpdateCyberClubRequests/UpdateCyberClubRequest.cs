using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubNameRequest : ICyberClubRequest, INameRequest
    {
        [Required] public Guid CyberClubId { get; set; } 
        [Required] public string Name { get; set; } = string.Empty; 
        
    }
}