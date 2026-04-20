using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface ISeatTypeRepository : IRepository<SeatType>
    {
        /// <summary>
        /// L?y danh sách seat types active (search + pagination)
        /// Dùng cho user - ch? th?y active
        /// </summary>
        Task<PagedResult<SeatType>> GetAllActiveSeatTypesAsync(
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        /// <summary>
        /// L?y danh sách t?t c? seat types cho admin (bao g?m c? inactive)
        /// Dùng cho admin - th?y c? active + inactive
        /// H? tr? filter by IsActive
        /// </summary>
        Task<PagedResult<SeatType>> GetAllSeatTypesForAdminAsync(
            string? search,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}
