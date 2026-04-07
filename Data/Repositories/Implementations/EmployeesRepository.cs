using System.Linq.Expressions;
using System.Security.Claims;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class EmployeesRepository : BaseRepository<EmployeeEntity>, IEmployeesRepository
    {
        public EmployeesRepository(AppDbContext dbContext) : base(dbContext) 
        {
            
        }

        public async Task<List<EmployeeEntity>> GetWithDetailsByExpressionAsync(
            Expression<Func<EmployeeEntity, bool>> predicate,
            CancellationToken token = default
            )
        {
            // Костыль, надо подкмать, поменять
            return await _dbSet
                .AsNoTracking()
                .Include(e => e.CyberClub)
                .Where(predicate)
                .ToListAsync(token);
        }

        public async Task SetZeroBonusesAsync(CancellationToken token = default)
        {
            await _dbSet.ExecuteUpdateAsync(ub =>
            {
                ub.SetProperty(e => e.Bonus, 0);
            },
            token);
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