using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GNS.Data.Repositories.Implementations
{
    public class RefreshTokensRepository(AppDbContext dbcontext)
        : BaseRepository<RefreshTokenEntity>(dbcontext), IRefreshTokensRepository
    {
        public async Task<RefreshTokenEntity?> GetByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.UserId == userId, token);
        }

        public async Task<RefreshTokenEntity?> GetWithDetailsAsync(Expression<Func<RefreshTokenEntity, bool>> expression, CancellationToken token = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(expression, token);
        }
    }
}