using GNS.Data.Entities;
using GNS.Enums;


namespace GNS.Data.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<OrderEntity> GetByIdAsync(Guid orderId, CancellationToken token = default);
        Task<List<OrderEntity>> GetByUserIdAsync(Guid userId, CancellationToken token = default);
        Task<List<OrderEntity>> GetByDateAsync(DateTime date, CancellationToken token = default);


        Task<List<OrderEntity>> GetOrdersOfGamingPlaceByDateAsync(
             Guid gamingPlaceId,
             DateTime date,
             CancellationToken token = default);
        Task CreateOrderAsync(
            Guid userId,
            Guid gamingPlaceId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken token = default
        );
        Task UpdateStatusAsync(Guid orderId, OrderStatus status, CancellationToken token = default);

    }
}