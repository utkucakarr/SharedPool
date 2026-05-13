using Microsoft.EntityFrameworkCore.Storage;
using SharedPool.Domain.Interfaces;
using SharedPool.Infrastructure.Contexts;

namespace SharedPool.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SharedPoolDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(SharedPoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Interceptor'ımız (AuditableEntityInterceptor) burada otomatik devreye girecek!
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if(_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _currentTransaction?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
