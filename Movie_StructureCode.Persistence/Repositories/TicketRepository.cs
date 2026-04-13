using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// L?y t?t c? ticket thu?c booking kèm ShowingSeat ? Seat ? SeatType.
        /// </summary>
        public async Task<IEnumerable<Ticket>> GetByBookingAsync(
            Guid bookingId,
            CancellationToken ct = default)
            => await _context.Tickets
                .AsNoTracking()
                .Include(t => t.ShowingSeat!)
                    .ThenInclude(ss => ss.Seat!)
                        .ThenInclude(s => s.SeatType)
                .Include(t => t.ShowingSeat!)
                    .ThenInclude(ss => ss.Showing!)
                        .ThenInclude(s => s.Movie)
                .Where(t => t.BookingId == bookingId)
                .OrderBy(t => t.DateCreate)
                .ToListAsync(ct);
    }
}
