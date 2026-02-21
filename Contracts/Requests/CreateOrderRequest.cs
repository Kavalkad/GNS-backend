using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class CreateOrderRequest
    {
        [Required] public Guid GamingPlaceId { get; set; }
        [Required] public string DateTime { get; set; } = string.Empty;
        [Required] public int Duration { get; set; }

    }
}