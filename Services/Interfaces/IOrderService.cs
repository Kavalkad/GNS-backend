using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface IOrderService
    {
        Task<TimeSlotDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken token = default);
        Task<List<OrderDto>> GetActiveOrdersAsync(CancellationToken token = default);
        Task<List<OrderEntity>> GetByDateAndGamingPlaceAsync(
            DateTime date,
            Guid gamingPlaceId,
            CancellationToken token = default
            );
        Task<List<OrderDto>> GetTodaysOrdersAsync(CancellationToken token = default);
        Task<List<OrderDto>> GetByUserEmailAsync(string email, CancellationToken token = default);
        Task<List<OrderDto>> GetByUserNameAsync(string userName, CancellationToken token = default);
        Task UpdateOrderStatusAsync(
            Guid orderId,
            string status,
            CancellationToken token = default);
       
    }
}