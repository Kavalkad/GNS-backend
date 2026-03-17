using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class DeleteGameGPsRequest
    {

        [Required] public Guid GameId  { get; set; }
        [Required] public string EquipmentName { get; set; } = string.Empty;
    }
}