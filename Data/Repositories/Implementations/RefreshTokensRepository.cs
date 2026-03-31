using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class RefreshTokensRepository(AppDbContext dbcontext) 
        : BaseRepository<RefreshTokenEntity>(dbcontext), IRefreshTokensRepository
    {
       
    }
}