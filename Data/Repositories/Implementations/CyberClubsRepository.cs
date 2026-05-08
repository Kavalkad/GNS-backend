using System.Linq.Expressions;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class CyberClubsRepository(AppDbContext dbcontext)
        : BaseRepository<CyberClubEntity>(dbcontext), ICyberClubsRepository
    {
        public async Task<CyberClubEntity?> GetWithDetailsAsync(Guid cyberClubId, CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking()
                .Include(cc => cc.GamingPlaces)
                .FirstOrDefaultAsync(cc => cc.Id == cyberClubId, token);
        }
    }
}