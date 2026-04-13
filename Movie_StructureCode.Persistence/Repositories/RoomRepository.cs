using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }

        // ?? USER ????????????????????????????????????????????????????????????

        /// <summary>
        /// L?y room kèm toàn b? seat layout (Seat + SeatType).
        /// </summary>
        public async Task<Room?> GetRoomWithSeatsAsync(
            Guid roomId,
            CancellationToken ct = default)
            => await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Seats!)
                    .ThenInclude(s => s.SeatType)
                .FirstOrDefaultAsync(r => r.Id == roomId, ct);

        // ?? ADMIN ???????????????????????????????????????????????????????????

        /// <summary>
        /// L?y danh sách room theo theater, h? tr? search theo tên và phân trang.
        /// </summary>
        public async Task<PagedResult<Room>> GetRoomsByTheaterAsync(
            Guid? theaterId,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Rooms
                .AsNoTracking()
                .Include(r => r.Theater)
                .AsQueryable();

            if (theaterId.HasValue)
                query = query.Where(r => r.TheaterId == theaterId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(r => r.Name.Contains(search));

            query = query.OrderBy(r => r.Name);

            return await PagedResult<Room>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
