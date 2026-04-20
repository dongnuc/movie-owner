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
        /// Lấy dánh sách rạp kèm count suất chiếu theo khoảng thời gian (dùng để xem lịch rạp).
        /// </summary>
        Task<IEnumerable<(Theater theater, int count)>> CountShowingByMovieWithDateAsync(
            Guid movieId,
            DateTime date,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy showing theo movie (bao gồm cả inactive) để admin quản lý.
        /// </summary>
        Task<IEnumerable<Showing>> GetByMovieAsync(
            Guid movieId,
            CancellationToken ct = default);

    }
}