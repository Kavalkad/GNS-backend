using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class DeleteGamingPlacesRequest
    {
        [Required] public string CyberClubId { get; set; } = string.Empty;
        // [Required] public string CyberClubName { get; set; } = string.Empty;
        [Required] public string EquipmentName { get; set; } = string.Empty;
    }
}