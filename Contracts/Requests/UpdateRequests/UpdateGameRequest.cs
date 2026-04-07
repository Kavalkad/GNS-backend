using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateGameTitleRequest : IGameRequest, INewTitleRequest
    {
        [Required] public string GameId { get; set; } = string.Empty;
        [Required] public string NewTitle { get; set; } = string.Empty;

    }
}