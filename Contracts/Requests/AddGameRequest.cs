using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Requests
{
    public record class AddGameRequest
    {
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public bool OnPC { get; set; }
        [Required] public bool OnPlayStation { get; set; }
        [Required] public bool OnXbox { get; set; }
    }
}