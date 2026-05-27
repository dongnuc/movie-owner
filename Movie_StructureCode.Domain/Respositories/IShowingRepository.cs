using Movie_StructureCode.Contract.Abstractions.Shared;
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

        /// <summary>
        /// Lấy danh sách showing cho admin theo rạp (theater)
        /// Hỗ trợ tìm kiếm theo title phim, lọc theo ngày bắt đầu, lọc theo trạng thái active, và phân trang
        /// </summary>
        Task<PagedResult<Showing>> GetShowingsForAdminByTheaterAsync(
            Guid theaterId,
            string? movieTitle = null,
            DateTime? timeStart = null,
            bool? isActive = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default);

        // ===============================
        // VALIDATION METHODS
        // ===============================

        /// <summary>
        /// Kiểm tra một Showing có hợp lệ để thao tác hay không.
        /// Hợp lệ khi: tồn tại, đang active, và chưa quá giờ chiếu so với thời điểm hiện tại.
        /// </summary>
        /// <param name="showingId">ID của suất chiếu cần kiểm tra.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        ///   <c>(true, null)</c>              – Showing tồn tại và chưa quá giờ.<br/>
        ///   <c>(false, "NotFound")</c>       – Không tìm thấy hoặc bị inactive.<br/>
        ///   <c>(false, "Expired")</c>        – Showing đã quá giờ chiếu.
        /// </returns>
        Task<(bool IsValid, string? FailReason)> IsShowingValidAsync(
            Guid showingId,
            CancellationToken ct = default);
    }
}