using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests.Implementations
{
    public class UpdateGamingPlacePricePerHourRequest : IGamingPlaceRequest, IPricePerHourRequest
    {
        [Required] public Guid GamingPlaceId { get; set; }
        [Required] public decimal PricePerHour { get; set; }
    }
}