using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Requests
{
    public class CreateGameGPsRequest
    {
        [Required] public Guid GameId { get; set; }
        [Required] public List<string> EquipmentNames { get; set; } = [];
     }
}