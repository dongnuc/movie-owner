using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class ShowingSeatRepository : Repository<ShowingSeat>, IShowingSeatRepository
    {
        // Th?i gian lock m?c ??nh: 10 phút
        private static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(10);

        public ShowingSeatRepository(AppDbContext context) : base(context) { }

        // ?? USER ?????????????????????????????????????????????????????????????

        /// <summary>
        /// L?y toàn b? gh? c?a m?t su?t chi?u kèm thông tin Seat + SeatType.
        /// </summary>
        public async Task<IEnumerable<ShowingSeat>> GetByShowingAsync(
            Guid showingId,
            CancellationToken ct = default)
            => await _context.ShowingSeats
                .AsNoTracking()
                .Include(ss => ss.Seat!)
                    .ThenInclude(s => s.SeatType)
                .Where(ss => ss.ShowingId == showingId)
                .OrderBy(ss => ss.Seat!.RowIndex)
                    .ThenBy(ss => ss.Seat!.ColIndex)
                .ToListAsync(ct);

        /// <summary>
        /// L?y nhi?u ShowingSeat theo danh sách Id (dùng khi user ch?n nhi?u gh?).
        /// </summary>
        public async Task<IEnumerable<ShowingSeat>> GetByIdsAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default)
            => await _context.ShowingSeats
                .Where(ss => seatIds.Contains(ss.Id))
                .ToListAsync(ct);

        /// <summary>
        /// Ki?m tra t?t c? gh? trong danh sách có ?ang Available không.
        /// </summary>
        public async Task<bool> AreSeatsAvailableAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default)
        {
            var ids    = seatIds.ToList();
            var now    = DateTime.UtcNow;

            var unavailable = await _context.ShowingSeats
                .Where(ss =>
                    ids.Contains(ss.Id) &&
                    (ss.Status == ShowingSeatStatus.Booked ||
                     (ss.Status == ShowingSeatStatus.Locked && ss.LockedUntil > now)))
                .AnyAsync(ct);

            return !unavailable;
        }

        /// <summary>
        /// Lock các gh? trong th?i gian LockDuration.
        /// Ch? lock gh? ?ang Available (ho?c lock ?ã h?t h?n).
        /// </summary>
        public async Task LockSeatsAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default)
        {
            var ids  = seatIds.ToList();
            var now  = DateTime.UtcNow;
            var until = now.Add(LockDuration);

            var seats = await _context.ShowingSeats
                .Where(ss =>
                    ids.Contains(ss.Id) &&
                    (ss.Status == ShowingSeatStatus.Available ||
                     (ss.Status == ShowingSeatStatus.Locked && ss.LockedUntil <= now)))
                .ToListAsync(ct);

            foreach (var seat in seats)
            {
                seat.Status      = ShowingSeatStatus.Locked;
                seat.LockedUntil = until;
                seat.DateUpdate  = now;
            }
        }

        /// <summary>
        /// Release các gh? ?ang b? lock tr? v? Available.
        /// </summary>
        public async Task ReleaseSeatsAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default)
        {
            var ids = seatIds.ToList();
            var now = DateTime.UtcNow;

            var seats = await _context.ShowingSeats
                .Where(ss =>
                    ids.Contains(ss.Id) &&
                    ss.Status == ShowingSeatStatus.Locked)
                .ToListAsync(ct);

            foreach (var seat in seats)
            {
                seat.Status      = ShowingSeatStatus.Available;
                seat.LockedUntil = null;
                seat.DateUpdate  = now;
            }
        }

        // ?? ADMIN ?????????????????????????????????????????????????????????????

        /// <summary>
        /// L?y toàn b? seat map (gh? + tr?ng thái) c?a m?t su?t chi?u.
        /// </summary>
        public async Task<IEnumerable<ShowingSeat>> GetSeatMapAsync(
            Guid showingId,
            CancellationToken ct = default)
            => await _context.ShowingSeats
                .AsNoTracking()
                .Include(ss => ss.Seat!)
                    .ThenInclude(s => s.SeatType)
                .Where(ss => ss.ShowingId == showingId)
                .OrderBy(ss => ss.Seat!.RowIndex)
                    .ThenBy(ss => ss.Seat!.ColIndex)
                .ToListAsync(ct);

        /// <summary>
        /// ??m s? gh? ?ã ???c ??t (Booked) trong m?t su?t chi?u.
        /// </summary>
        public async Task<int> CountBookedSeatsAsync(
            Guid showingId,
            CancellationToken ct = default)
            => await _context.ShowingSeats
                .CountAsync(ss =>
                    ss.ShowingId == showingId &&
                    ss.Status == ShowingSeatStatus.Booked, ct);
    }
}
