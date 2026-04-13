using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface IShowingSeatRepository : IRepository<ShowingSeat>
    {

        ///////// USER////////////
        /// <summary>
        /// Lấy toàn bộ ghế của một suất chiếu
        /// </summary>
        Task<IEnumerable<ShowingSeat>> GetByShowingAsync(
            Guid showingId,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy ghế theo danh sách id (dùng khi user chọn nhiều ghế)
        /// </summary>
        Task<IEnumerable<ShowingSeat>> GetByIdsAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default);

        /// <summary>
        /// Kiểm tra ghế còn available không
        /// </summary>
        Task<bool> AreSeatsAvailableAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default);

        /// <summary>
        /// Lock ghế khi user chọn
        /// </summary>
        Task LockSeatsAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default);

        /// <summary>
        /// Release ghế khi user bỏ chọn hoặc timeout
        /// </summary>
        Task ReleaseSeatsAsync(
            IEnumerable<Guid> seatIds,
            CancellationToken ct = default);

        ////// ADMIN ///////////////
        /// <summary>
        /// Lấy toàn bộ seat map của showing
        /// </summary>
        Task<IEnumerable<ShowingSeat>> GetSeatMapAsync(
            Guid showingId,
            CancellationToken ct = default);

        /// <summary>
        /// Thống kê ghế
        /// </summary>
        Task<int> CountBookedSeatsAsync(
            Guid showingId,
            CancellationToken ct = default);

    }
}
