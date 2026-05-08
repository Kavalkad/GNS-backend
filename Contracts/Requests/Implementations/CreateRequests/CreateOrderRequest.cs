using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class CreateOrderRequest : ITimeSpanRequest, IGamingPlaceRequest
    {

        [Required]
        public Guid GamingPlaceId { get; set; } 

        [Required]
        public DateTime DateTimeStart { get; set; } 

        [Required]
        public DateTime DateTimeEnd { get; set; } 

    }
}