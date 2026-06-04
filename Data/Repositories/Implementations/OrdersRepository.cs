using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;

namespace GNS.Data.Repositories.Implementations
{
    public class OrdersRepository(AppDbContext dbcontext) 
        : BaseRepository<OrderEntity>(dbcontext), IOrdersRepository
    {
    }
}