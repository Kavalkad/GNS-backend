using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public class UpdateGameRequest
    {
        [Required] public Guid GameId { get; set; } 
        [Required] public string NewTitle { get; set; } = string.Empty;
    }
}