using System.Linq.Expressions;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IRefreshTokensRepository : IRepository<RefreshTokenEntity>
    {
        Task<RefreshTokenEntity?> GetByUserIdAsync(Guid userId, CancellationToken token = default);
        Task<RefreshTokenEntity?> GetWithDetailsAsync(Expression<Func<RefreshTokenEntity, bool>> expression, CancellationToken token = default);
    }
}