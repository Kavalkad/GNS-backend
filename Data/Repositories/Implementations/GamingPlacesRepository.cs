using System.Linq.Expressions;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GamingPlacesRepository(AppDbContext dbcontext)
        : BaseRepository<GamingPlaceEntity>(dbcontext), IGamingPlacesRepository
    {
        public async Task<int> CountAsync(Expression<Func<GamingPlaceEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbSet.CountAsync(predicate, token);
        }
        public async Task<GamingPlaceEntity?> GetWithDetailsAsync(Guid gamingPlaceId, CancellationToken token = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(gp => gp.CyberClub)
                .FirstOrDefaultAsync(gp => gp.Id == gamingPlaceId, token);
        }
    }
}