using System.Linq.Expressions;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IUsersRepository : IRepository<UserEntity>
    {
        Task<bool> ContainsExpressionAsync(Expression<Func<UserEntity, bool>> predicate, CancellationToken token = default);
    }
}