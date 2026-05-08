using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubCityRequest : ICyberClubRequest, ICityRequest
    {
        [Required] public Guid CyberClubId { get; set; } 
        [Required] public string City { get; set; } = string.Empty;
        
    }
}