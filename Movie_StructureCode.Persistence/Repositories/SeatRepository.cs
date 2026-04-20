using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context) { }

        public async Task<Dictionary<Guid, int>> CountSeatUnitsByRoomIdsAsync(
        List<Guid> roomIds,
        CancellationToken cancellationToken = default)
        {
            if (roomIds == null || roomIds.Count == 0)
                return new Dictionary<Guid, int>();

            return await _context.Seats
                .AsNoTracking()
                .Where(s => roomIds.Contains(s.RoomId)
                         && s.StatusSeat == StatusSeat.Active)
                .GroupBy(s => s.RoomId)
                .Select(g => new
                {
                    RoomId = g.Key,
                    Count = g
                        .GroupBy(s => s.GroupId ?? s.Id) 
                        .Count()
                })
                .ToDictionaryAsync(x => x.RoomId, x => x.Count, cancellationToken);
        }
    }
}
