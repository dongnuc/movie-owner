using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface IShowingRepository : IRepository<Showing>
    {
        // ===============================
        // USER METHODS
        // ===============================

        /// <summary>
        /// Lấy danh sách showing đang active theo movie.
        /// </summary>
        Task<IEnumerable<Showing>> GetShowingsActiveAsync(
        Guid movieId,
        Guid theaterId,
        DateTime? date,
        CancellationToken ct = default);     

        // ===============================
        // ADMIN METHODS
        // ===============================

        /// <summary>
        /// Lấy danh sách showing theo phòng (dùng để xem lịch phòng).
        /// </summary>
        Task<IEnumerable<Showing>> GetByRoomAsync(
            Guid roomId,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy tất cả showing trong khoảng thời gian (dùng để quản lý lịch chiếu).
        /// </summary>
        Task<IEnumerable<Showing>> GetByDateRangeAsync(
            DateTime from,
            DateTime to,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy showing theo movie (bao gồm cả inactive) để admin quản lý.
        /// </summary>
        Task<IEnumerable<Showing>> GetByMovieAsync(
            Guid movieId,
            CancellationToken ct = default);
    }
}