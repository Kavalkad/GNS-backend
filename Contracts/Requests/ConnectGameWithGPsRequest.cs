using System.ComponentModel.DataAnnotations;
using GNS.Enums;

namespace GNS.Contracts.Requests
{
    public class AddGameGPsRequest
    {
        [Required] public Guid GameId { get; set; }
        [Required] public string EquipmentName { get; set; } = null!;
     }
}