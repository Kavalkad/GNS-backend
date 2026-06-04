using Microsoft.EntityFrameworkCore;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using System.Linq.Expressions;

namespace GNS.Data.Repositories.Implementations
{
    public class UsersRepository(AppDbContext dbcontext)
        : BaseRepository<UserEntity>(dbcontext), IUsersRepository
    {
        public async Task<bool> ContainsExpressionAsync(Expression<Func<UserEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate, token);
        }
    }
}