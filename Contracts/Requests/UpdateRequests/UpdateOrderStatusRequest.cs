using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public class UpdateOrderStatusRequest
    {
        [Required] public Guid OrderId { get; set; }
        [Required] public string NewOrderStatus { get; set; } = string.Empty;

    }
}