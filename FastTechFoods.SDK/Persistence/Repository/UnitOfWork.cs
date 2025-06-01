using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FastTechFoods.SDK.Persistence.Repository
{
    public class UnitOfWork(DbContext context) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;
        private readonly Dictionary<Type, object> _repositories = new();

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.TryGetValue(type, out var repo))
                return (IRepository<T>)repo;

            var repositoryInstance = new Repository<T>(context);
            _repositories.Add(type, repositoryInstance);

            return repositoryInstance;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return;

            _transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}