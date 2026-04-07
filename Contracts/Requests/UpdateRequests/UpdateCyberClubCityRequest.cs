using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubCityRequest : ICyberClubRequest, ICityRequest
    {
        [Required] public string CyberClubId { get; set; } = string.Empty;
        [Required] public string City { get; set; } = string.Empty; 
        
    }
}