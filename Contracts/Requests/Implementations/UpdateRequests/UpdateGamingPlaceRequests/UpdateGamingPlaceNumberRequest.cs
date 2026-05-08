using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests.Implementations
{
    public class UpdateGamingPlaceNumberRequest : IGamingPlaceRequest, INumberRequest
    {
        [Required] public Guid GamingPlaceId { get; set; }
        [Required] public int Number { get; set; }
    }
}