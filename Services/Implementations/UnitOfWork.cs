using GNS.Data;
using GNS.Data.Entities;
using GNS.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace GNS.Services.Implementations
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbcontext;
        private IDbContextTransaction? _transaction;

        public bool HasActiveTransaction => _transaction != null;

        public UnitOfWork(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbcontext.SaveChangesAsync(cancellationToken);
        }
        

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (HasActiveTransaction)
            {
                throw new InvalidOperationException("Транзакция уже начата");
            }

            _transaction = await _dbcontext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Нет активной транзакции");
            }

            try
            {
                await _dbcontext.SaveChangesAsync(cancellationToken);

                await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (!HasActiveTransaction)
            {
                throw new InvalidOperationException("Нет активной транзакции");
            }

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (HasActiveTransaction)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }


        public async ValueTask DisposeAsync()
        {
            if (HasActiveTransaction)
            {
                await _transaction.DisposeAsync();
            }
            await _dbcontext.DisposeAsync();
        }
    }

}