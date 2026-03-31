using System.Linq.Expressions;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        // CREATE
        Task AddAsync(TEntity entity, CancellationToken token = default);
        // Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        // READ
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<List<TEntity>> GetAllAsync(CancellationToken token = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        // UPDATE
        void Update(TEntity entity);
        // void UpdateRange(IEnumerable<TEntity> entities);

        // DELETE
        void Delete(TEntity entity);
        // void DeleteRange(IEnumerable<TEntity> entities);
        Task DeleteByIdAsync(Guid id, CancellationToken token = default);
    }
}