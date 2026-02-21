using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class DeleteCCGamingPlacesRequest
    {
       // [Required] public Guid CyberClubId { get; set; }
        [Required] public string CyberClubName { get; set; } = string.Empty;
        [Required] public string EquipmentName { get; set; } = string.Empty;
    }
}