using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        // ======================================
        // USER METHODS
        // ======================================

        /// <summary>
        /// Lấy danh sách category đang active (có phân trang).
        /// </summary>
        Task<PagedResult<Category>> GetAllActiveAsync(
            CancellationToken ct = default);

        // ======================================
        // ADMIN METHODS
        // ======================================

        /// <summary>
        /// Lấy tất cả category (bao gồm inactive) có phân trang.
        /// </summary>
        Task<PagedResult<Category>> GetAllAsync(
            Guid? id,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

    }
}