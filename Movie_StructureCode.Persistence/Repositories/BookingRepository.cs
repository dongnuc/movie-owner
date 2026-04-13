using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }

        // ── USER ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy danh sách booking của user kèm Showing + Movie cơ bản, có phân trang.
        /// Chi tiết vé (Ticket) sẽ được load riêng qua TicketRepository.
        /// </summary>
        public async Task<IEnumerable<Booking>> GetByUserAsync(
            Guid appUserId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
            => await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Showing!)
                    .ThenInclude(s => s.Movie)
                .Where(b => b.AppUserId == appUserId)
                .OrderByDescending(b => b.DateCreate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

        // ── ADMIN ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy danh sách booking theo suất chiếu, tìm kiếm theo UserName/Email, có phân trang.
        /// Chi tiết vé (Ticket) sẽ được load riêng qua TicketRepository.
        /// </summary>
        public async Task<(IEnumerable<Booking> Bookings, int TotalCount)> GetByShowingAsync(
            Guid showingId,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Bookings
                .AsNoTracking()
                .Include(b => b.AppUser)
                .Include(b => b.Showing)
                .Where(b => b.ShowingId == showingId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(b =>
                    b.AppUser != null &&
                    (b.AppUser.UserName!.Contains(search) ||
                     b.AppUser.Email!.Contains(search)));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(b => b.DateCreate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

    }
}
