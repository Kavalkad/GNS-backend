using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class OrdersRepository : BaseRepository<OrderEntity>, IOrdersRepository
    {
        public OrdersRepository(AppDbContext dbcontext) : base(dbcontext){ }

       

    }
}