using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _dbcontext;
        public OrdersRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task CreateOrderAsync(
            Guid userId,
            Guid gamingPlaceId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken token = default)
        {
            var order = new OrderEntity
            {
                UserId = userId,
                GamingPlaceId = gamingPlaceId,
                DateTimeStart = startTime,
                DateTimeEnd = endTime,
                OrderStatus = OrderStatus.Booked
            };
            await _dbcontext.Orders.AddAsync(order, token);

        }

        public async Task<List<OrderEntity>> GetByDateAsync(DateTime date, CancellationToken token = default)
        {

            return await _dbcontext.Orders
                .AsNoTracking()
                .Where(o => o.DateTimeStart.Date == date)
                .ToListAsync();
        }

        public Task<OrderEntity> GetByIdAsync(Guid orderId, CancellationToken token = default)
        {
            ///!!!!!!!!!!!!!!
            throw new NotImplementedException();
        }

        public async Task<List<OrderEntity>> GetByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return await _dbcontext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.GamingPlace)
                .Include(o => o.GamingPlace.CyberClub)
                .ToListAsync();
        }

        public async Task<List<OrderEntity>> GetOrdersOfGamingPlaceByDateAsync(
            Guid gamingPlaceId,
            DateTime date,
            CancellationToken token = default)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Where(gp => gp.Id == gamingPlaceId)
                .Include(gp => gp.Orders)
                .SelectMany(gp => gp.Orders)
                .Where(o => o.DateTimeStart.Date == date)
                .ToListAsync(token);
        }

        public async Task UpdateStatusAsync(Guid orderId, OrderStatus status, CancellationToken token = default)
        {

            await _dbcontext.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(o => o.OrderStatus, status);
                });
        }
        
    }
}