using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateGameTitleRequest : IGameRequest, INewTitleRequest
    {
        [Required] public Guid GameId { get; set; } 
        [Required] public string NewTitle { get; set; } = string.Empty;

    }
}