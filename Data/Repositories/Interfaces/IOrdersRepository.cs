using GNS.Data.Entities;


namespace GNS.Data.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<OrderEntity> GetById(Guid orderId);
        Task<List<OrderEntity>> GetByUserId(Guid userId);
        Task<List<OrderEntity>> GetByDate(DateOnly date);


        Task<List<OrderEntity>> GetDateOrdersOfGamingPlace(
             Guid gamingPlaceId,
             DateOnly date,
             CancellationToken token = default);
        Task CreateOrderAsync(
            Guid userId,
            Guid gamingPlaceId,
            DateOnly date,
            TimeOnly startTime,
            int duration
        );
        Task UpdateStatus(Guid orderId, string status);
        
    }
}