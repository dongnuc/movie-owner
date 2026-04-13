using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        // ======================================
        // USER METHODS
        // ======================================

        /// <summary>
        /// Lấy danh sách booking của user (có phân trang).
        /// </summary>
        Task<IEnumerable<Booking>>GetByUserAsync(
            Guid appUserId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        // ======================================
        // ADMIN METHODS
        // ======================================

        /// <summary>
        /// Lấy danh sách booking theo suất chiếu (có search và phân trang).
        /// </summary>
        Task<(IEnumerable<Booking> Bookings, int TotalCount)> GetByShowingAsync(
            Guid showingId,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

    }
}