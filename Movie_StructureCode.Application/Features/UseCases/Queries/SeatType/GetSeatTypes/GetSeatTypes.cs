using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypes
{
    public static class GetSeatTypes
    {
        /// <summary>
        /// Query ?? l?y danh sách seat types (search + phân trang)
        /// </summary>
        public sealed record Query(
            string? Search,
            int PageNumber,
            int PageSize
        ) : IQuery<PagedResult<SeatTypeUserDto>>;

        /// <summary>
        /// Map Entity to User DTO
        /// </summary>
        public static SeatTypeUserDto ToDtoUser(Domain.Entities.SeatType st) =>
            new(st.Id, st.Name, st.Surcharge);

        /// <summary>
        /// Map Entity to Admin DTO
        /// </summary>
        public static SeatTypeAdminDto ToDtoAdmin(Domain.Entities.SeatType st) =>
            new(st.Id, st.Name, st.Surcharge, st.IsActive, st.DateCreate, st.DateUpdate);
    }
}
