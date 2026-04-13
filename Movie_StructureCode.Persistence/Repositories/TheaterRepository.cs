using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class TheaterRepository : Repository<Theater>, ITheaterRepository
    {
        public TheaterRepository(AppDbContext context) : base(context) { }

        // ?? USER ????????????????????????????????????????????????????????????

        /// <summary>
        /// L?y danh sách r?p có su?t chi?u c?a movie ?ó trong ngày ch? ??nh.
        /// </summary>
        public async Task<IEnumerable<Theater>> GetTheatersByMovieAsync(
            Guid movieId,
            DateTime date,
            CancellationToken ct = default)
        {
            var nextDay = date.Date.AddDays(1);

            return await _context.Theaters
                .AsNoTracking()
                .Where(t => t.IsActive &&
                            t.Rooms!.Any(r =>
                                r.Showings!.Any(s =>
                                    s.MovieId == movieId &&
                                    s.IsActive &&
                                    s.TimeStart >= date.Date &&
                                    s.TimeStart < nextDay)))
                .OrderBy(t => t.Name)
                .ToListAsync(ct);
        }
    }
}
