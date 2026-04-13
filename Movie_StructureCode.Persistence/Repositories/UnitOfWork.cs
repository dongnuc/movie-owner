using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }
}
