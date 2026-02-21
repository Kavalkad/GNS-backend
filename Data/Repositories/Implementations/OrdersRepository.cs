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
            DateOnly date,
            TimeOnly startTime,
            int duration)
        {
            var order = new OrderEntity
            {
                UserId = userId,
                GamingPlaceId = gamingPlaceId,
                Date = date,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(duration * 60),
                OrderStatus = OrderStatus.Booked
            };
            await _dbcontext.Orders.AddAsync(order);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<OrderEntity>> GetByDate(DateOnly date)
        {
            return await _dbcontext.Orders
                .AsNoTracking()
                .Where(o => o.Date == date)
                .ToListAsync();
        }

        public Task<OrderEntity> GetById(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderEntity>> GetByUserId(Guid userId)
        {
            return await _dbcontext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.GamingPlace)
                .Include(gp => gp.GamingPlace.CyberClub)
                .ToListAsync();
        }

        public async Task<List<OrderEntity>> GetDateOrdersOfGamingPlace(
            Guid gamingPlaceId,
            DateOnly date,
            CancellationToken token = default)
        {
            return await _dbcontext.GamingPlaces
                .AsNoTracking()
                .Where(gp => gp.Id == gamingPlaceId)
                .Include(gp => gp.Orders)
                .SelectMany(gp => gp.Orders)
                .Where(o => o.Date == date)
                .ToListAsync(token);
        }

        public async Task UpdateStatus(Guid orderId, string statusName)
        {
            var orderStatus = Enum.Parse<OrderStatus>(statusName);

            await _dbcontext.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(o => o.OrderStatus, orderStatus);
                });
        }
        
    }
}