namespace FastTechFoods.SDK.Persistence.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}