using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateGameOnRequest : IGameRequest, INewOnRequest
    {
        [Required] public string GameId { get; set; } = string.Empty;
        [Required] public bool NewOnValue { get; set; }
    }
}