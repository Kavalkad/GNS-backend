using System.Linq.Expressions;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity, CancellationToken token = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<List<TEntity>> GetAllAsync(CancellationToken token = default);

        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
        Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        void Update(TEntity entity);


        void Delete(TEntity entity);

        Task DeleteByIdAsync(Guid id, CancellationToken token = default);
    }
}