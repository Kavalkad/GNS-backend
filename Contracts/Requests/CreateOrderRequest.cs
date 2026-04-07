using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class CreateOrderRequest
    {
        [Required] public Guid CyberClubId { get; set; }
        [Required] public string GamingPlaceId { get; set; } = string.Empty;
        [Required] public string DateTimeStart { get; set; } = string.Empty;
        [Required] public string DateTimeEnd { get; set; } = string.Empty;

    }
}