using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class ShowingRepository : Repository<Showing>, IShowingRepository
    {
        public ShowingRepository(AppDbContext context) : base(context) { }

        // ?? USER ????????????????????????????????????????????????????????????

        /// <summary>
        /// L?y danh sách showing active theo movieId + theaterId,
        /// tu? ch?n l?c thêm theo ngày c? th?.
        /// </summary>
        public async Task<IEnumerable<Showing>> GetShowingsActiveAsync(
            Guid movieId,
            Guid theaterId,
            DateTime? date,
            CancellationToken ct = default)
        {
            var query = _context.Showings
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Room!)
                    .ThenInclude(r => r.Theater)
                .Where(s =>
                    s.IsActive &&
                    s.MovieId == movieId &&
                    s.Room!.TheaterId == theaterId)
                .AsQueryable();

            if (date.HasValue)
            {
                var from = date.Value.Date;
                var to   = from.AddDays(1);
                query = query.Where(s => s.TimeStart >= from && s.TimeStart < to);
            }

            return await query
                .OrderBy(s => s.TimeStart)
                .ToListAsync(ct);
        }

        // ?? ADMIN ???????????????????????????????????????????????????????????

        /// <summary>
        /// L?y toàn b? showing trong phòng (bao g?m c? inactive) ?? xem l?ch phòng.
        /// </summary>
        public async Task<IEnumerable<Showing>> GetByRoomAsync(
            Guid roomId,
            CancellationToken ct = default)
            => await _context.Showings
                .AsNoTracking()
                .Include(s => s.Movie)
                .Where(s => s.RoomId == roomId)
                .OrderBy(s => s.TimeStart)
                .ToListAsync(ct);

        /// <summary>
        /// L?y toàn b? showing trong kho?ng th?i gian ?? qu?n lý l?ch chi?u.
        /// </summary>
        public async Task<IEnumerable<Showing>> GetByDateRangeAsync(
            DateTime from,
            DateTime to,
            CancellationToken ct = default)
            => await _context.Showings
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Room!)
                    .ThenInclude(r => r.Theater)
                .Where(s => s.TimeStart >= from && s.TimeStart <= to)
                .OrderBy(s => s.TimeStart)
                .ToListAsync(ct);

        /// <summary>
        /// L?y t?t c? showing c?a movie (bao g?m c? inactive) ?? admin qu?n lý.
        /// </summary>
        public async Task<IEnumerable<Showing>> GetByMovieAsync(
            Guid movieId,
            CancellationToken ct = default)
            => await _context.Showings
                .AsNoTracking()
                .Include(s => s.Room!)
                    .ThenInclude(r => r.Theater)
                .Where(s => s.MovieId == movieId)
                .OrderByDescending(s => s.TimeStart)
                .ToListAsync(ct);
    }
}
