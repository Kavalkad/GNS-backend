using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Requests
{
    public record class AddGamingPlacesRequest
    {
        [Required] public Guid CyberClubId { get; set; } 
        [Required] public int Count { get; set; }
        [Required] public decimal PricePerHour { get; set; }
        [Required] public string EquipmentName { get; set; } = string.Empty;
    }
}