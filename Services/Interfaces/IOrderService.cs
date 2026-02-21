using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrder(CreateOrderRequest requset);
        Task<List<OrderDto>> GetActiveOrders();
        Task<List<OrderDto>> GetTodaysOrders();
        Task<List<OrderDto>> GetByUserEmail(string email);
        Task<List<OrderDto>> GetByUserName(string userName);
        Task UpdateOrderStatus(Guid orderId, string status);
       
    }
}