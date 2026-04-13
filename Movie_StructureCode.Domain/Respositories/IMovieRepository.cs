using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        // ======================================
        // USER METHODS
        // ======================================

        /// <summary>
        /// Lấy chi tiết movie(thông tin phim) kèm Category.
        /// </summary>
        Task<Movie?> GetByIdWithCategoryAsync(
            Guid id,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy phim kèm danh sách showing
        /// </summary>
        Task<Movie?> GetMovieWithShowingsAsync(Guid movieId, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách movie đang active 
        /// (search + category + pagination).
        /// Nếu search hoặc categoryId = null thì bỏ qua filter đó.
        /// </summary>
        Task<PagedResult<Movie>> GetMoviesForUserAsync(
            Guid? categoryId,
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);


        // ======================================
        // ADMIN METHODS
        // ======================================

        /// <summary>
        /// Lấy danh sách movie cho admin 
        /// (search + category + active + pagination).
        /// Nếu tham số null thì bỏ qua filter.
        /// </summary>
        Task<PagedResult<Movie>> GetMoviesForAdminAsync(
            Guid? categoryId,
            string? search,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}
