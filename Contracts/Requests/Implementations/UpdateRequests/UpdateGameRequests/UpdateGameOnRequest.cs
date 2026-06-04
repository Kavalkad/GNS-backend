using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;


namespace GNS.Contracts.Requests
{
    public class UpdateGameOnRequest : IGameRequest, INewOnRequest
    {
        [Required] public Guid GameId { get; set; }
        [Required] public bool NewOnValue { get; set; }
    }
}