using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        // ======================================
        // USER METHODS
        // ======================================

        /// <summary>
        /// Lấy room kèm seat layout
        /// </summary>
        Task<Room?> GetRoomWithSeatsAsync(Guid roomId, CancellationToken ct = default);

        // ======================================
        // ADMIN METHODS
        // ======================================

        /// <summary>
        /// Lấy danh sách room theo theater (search + pagination).
        /// </summary>
        Task<PagedResult<Room>> GetRoomsByTheaterAsync(
            Guid? theaterId,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}