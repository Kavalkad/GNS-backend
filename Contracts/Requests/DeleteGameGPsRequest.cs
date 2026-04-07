using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class DeleteGameGPsRequest
    {
        [Required] public string GameId { get; set; } = string.Empty;
        [Required] public string EquipmentName { get; set; } = string.Empty;
    }
}