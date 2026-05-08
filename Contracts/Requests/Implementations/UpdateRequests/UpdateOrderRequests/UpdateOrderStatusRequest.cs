using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateOrderStatusRequest : IOrderRequest
    {
        [Required] public Guid OrderId { get; set; }
        [Required] public string NewOrderStatus { get; set; } = string.Empty;

    }
}