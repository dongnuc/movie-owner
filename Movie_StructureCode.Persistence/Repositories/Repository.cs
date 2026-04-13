using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;
using System.Linq.Expressions;

namespace Movie_StructureCode.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet   = context.Set<T>();
        }

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);

        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicate, ct);

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _dbSet.AddAsync(entity, ct);

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
            => await _dbSet.AddRangeAsync(entities, ct);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Remove(T entity)
            => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);
    }
}
