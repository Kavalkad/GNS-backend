using System.Linq.Expressions;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class EmployeesRepository(AppDbContext dbContext) : BaseRepository<EmployeeEntity>(dbContext), IEmployeesRepository
    {
        public async Task<List<EmployeeEntity>> GetWithDetailsByExpressionAsync(
            Expression<Func<EmployeeEntity, bool>> predicate,
            CancellationToken token = default
            )
        {
            return await _dbSet
                .AsNoTracking()
                .Include(e => e.CyberClub)
                .Where(predicate)
                .ToListAsync(token);
        }

        public async Task SetZeroBonusesAsync(CancellationToken token = default)
        {
            await _dbSet
                .Where(e => e.Bonus != 0)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(e => e.Bonus, 0);
                }, token);
        }

        public async Task SetZeroPenaltiesAsync(CancellationToken token = default)
        {
            await _dbSet.ExecuteUpdateAsync(ub =>
            {
                ub.SetProperty(e => e.Penalty, 0);
            },
            token);
        }
    }
}