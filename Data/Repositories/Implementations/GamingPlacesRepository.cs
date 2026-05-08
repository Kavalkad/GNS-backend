using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GamingPlacesRepository(AppDbContext dbcontext)
        : BaseRepository<GamingPlaceEntity>(dbcontext), IGamingPlacesRepository
    {
        public async Task<GamingPlaceEntity?> GetByIdWithDetailsAsync(
            Guid gamingPlaceId,
            CancellationToken token = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(gp => gp.CyberClub)
                .FirstOrDefaultAsync(gp => gp.Id == gamingPlaceId, token);
        }
    }
}