using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class CreateGameRequest
    {
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public bool OnPC { get; set; }
        [Required] public bool OnPlayStation { get; set; }
        [Required] public bool OnXbox { get; set; }
    }
}