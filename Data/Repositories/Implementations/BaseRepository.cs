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
                ?? throw new EntityNotFoundException(nameof(TEntity));

            _dbSet.Remove(entity);
        }
    }
}