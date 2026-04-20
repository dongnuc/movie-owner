using Movie_StructureCode.Application.Abstractions.Services.Cache;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetAllSeatTypesForAdmin
{
    public static class GetAllSeatTypesForAdmin
    {
        /// <summary>
        /// Query ?? l?y danh sách t?t c? seat types cho admin (bao g?m c? inactive)
        /// Admin có th? th?y toàn b? seat types (active + inactive)
        /// H? tr?: search + phân trang + filter by isActive
        /// </summary>
        public sealed record Query(
            string? Search,
            bool? IsActive,
            int PageNumber,
            int PageSize
        ) : IQuery<PagedResult<SeatTypeAdminDto>>, ICacheableQuery
        {
            public bool BypassCache => !string.IsNullOrEmpty(Search) || PageNumber > 1;

            public string CacheKey => "seattype:admin:list:default";

            public TimeSpan? Expiration => TimeSpan.FromMinutes(5);

            public Type ValueType => typeof(PagedResult<SeatTypeAdminDto>);
        };

        /// <summary>
        /// Map Entity to Admin DTO (??y ?? thông tin)
        /// </summary>
        public static SeatTypeAdminDto ToDtoAdmin(Domain.Entities.SeatType st) =>
            new(st.Id, st.Name, st.Surcharge, st.IsActive, st.DateCreate, st.DateUpdate);
    }
}
