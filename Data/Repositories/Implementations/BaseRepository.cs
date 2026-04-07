using System.Linq.Expressions;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _dbcontext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _dbSet = _dbcontext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken token = default)
        {
            await _dbSet.AddAsync(entity, token);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            await _dbSet.AddRangeAsync(entities, token);
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(token);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, token);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate, token);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            var entity = await GetByIdAsync(id, token)
                ?? throw new EntityNotFoundException(nameof(TEntity), id.ToString());

            _dbSet.Remove(entity);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, token);
        }

        public async Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(token);
        }
    }
}