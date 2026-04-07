using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public class UpdateCCGamingPlacesRequest
    {
        [Required] public string CyberClubName { get; set; } = string.Empty;
        public int NewCount { get; set; }
        public decimal NewPricePerHour { get; set; }
        public string NewEquipmentName { get; set; } = string.Empty;   
    }
}