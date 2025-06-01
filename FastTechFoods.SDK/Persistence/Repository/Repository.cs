using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastTechFoods.SDK.Persistence.Repository
{
    public class Repository<T>(DbContext context) : IRepository<T>
        where T : class
    {
        protected readonly DbContext _context = context;
        private readonly DbSet<T> _entities = context.Set<T>();

        public async Task<T?> GetByIdAsync(object id) => await _entities.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _entities.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _entities.AddAsync(entity);

        public void Update(T entity) => _entities.Update(entity);

        public void Remove(T entity) => _entities.Remove(entity);
    }

}
