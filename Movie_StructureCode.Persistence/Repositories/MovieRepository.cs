using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(AppDbContext context) : base(context) { }

        // ── USER ────────────────────────────────────────────────────────────

        public async Task<Movie?> GetByIdWithCategoryAsync(
            Guid id,
            CancellationToken ct = default)
            => await _context.Movies
                .AsNoTracking()
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id, ct);

        public async Task<Movie?> GetMovieWithShowingsAsync(
            Guid movieId,
            CancellationToken ct = default)
            => await _context.Movies
                .AsNoTracking()
                .Include(m => m.Showings)
                .FirstOrDefaultAsync(m => m.Id == movieId, ct);

        public async Task<PagedResult<Movie>> GetMoviesForUserAsync(
            Guid? categoryId,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Movies
                .AsNoTracking()
                .Include(m => m.Category)
                .Where(m => m.IsActive)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(m => m.Title.Contains(search));

            query = query.OrderBy(m => m.Title);

            return await PagedResult<Movie>.CreateAsync(query, pageNumber, pageSize);
        }

        // ── ADMIN ───────────────────────────────────────────────────────────

        public async Task<PagedResult<Movie>> GetMoviesForAdminAsync(
            Guid? categoryId,
            string? search,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Movies
                .AsNoTracking()
                .Include(m => m.Category)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(m => m.Title.Contains(search));

            if (isActive.HasValue)
                query = query.Where(m => m.IsActive == isActive.Value);

            query = query.OrderByDescending(m => m.DateCreate);

            return await PagedResult<Movie>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
