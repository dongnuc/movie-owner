using Microsoft.EntityFrameworkCore;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class SeatTypeRepository : Repository<SeatType>, ISeatTypeRepository
    {
        public SeatTypeRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// L?y danh sách seat types active (search + pagination)
        /// Dùng cho user - ch? th?y active
        /// </summary>
        public async Task<PagedResult<SeatType>> GetAllActiveSeatTypesAsync(
            string? search,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.SeatTypes
                .AsNoTracking()
                .Where(st => st.IsActive)
                .AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(st => st.Name.Contains(search));

            // Order by name
            query = query.OrderBy(st => st.Name);

            // Create paginated result
            return await PagedResult<SeatType>.CreateAsync(query, pageNumber, pageSize);
        }

        /// <summary>
        /// L?y danh sách t?t c? seat types cho admin (bao g?m c? inactive)
        /// Dùng cho admin - th?y c? active + inactive
        /// H? tr? filter by IsActive
        /// </summary>
        public async Task<PagedResult<SeatType>> GetAllSeatTypesForAdminAsync(
            string? search,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.SeatTypes
                .AsNoTracking()
                .AsQueryable();

            // Apply IsActive filter if provided
            if (isActive.HasValue)
                query = query.Where(st => st.IsActive == isActive.Value);

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(st => st.Name.Contains(search));

            // Order by name
            query = query.OrderBy(st => st.Name);

            // Create paginated result
            return await PagedResult<SeatType>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
