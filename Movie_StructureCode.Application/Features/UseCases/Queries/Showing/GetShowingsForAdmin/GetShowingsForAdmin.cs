using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using System;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsForAdmin
{
    public static class GetShowingsForAdmin
    {
        /// <summary>
        /// Query ?? l?y danh sách su?t chi?u cho admin theo r?p
        /// H? tr? tìm ki?m theo title phim, l?c theo ngày b?t ??u, l?c theo tr?ng thái active, và phân trang
        /// </summary>
        public sealed record Query(
            Guid TheaterId,
            string? MovieTitle = null,
            DateTime? TimeStart = null,
            bool? IsActive = null,
            int PageNumber = 1,
            int PageSize = 10
        ) : IQuery<PagedResult<ShowingAdminDto>>;
    }
}
