using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        // ?? USER ????????????????????????????????????????????????????????????

        public async Task<PagedResult<Category>> GetAllActiveAsync(
            CancellationToken ct = default)
        {
            var query = _context.Categories
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name);

            return await PagedResult<Category>.CreateAsync(query,
                PagedResult<Category>.DefaultPageIndex,
                PagedResult<Category>.UperPageSize);
        }

        // ?? ADMIN ???????????????????????????????????????????????????????????

        public async Task<PagedResult<Category>> GetAllAsync(
            Guid? id,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .AsQueryable();

            if (id.HasValue)
                query = query.Where(c => c.Id == id.Value);

            return await PagedResult<Category>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
