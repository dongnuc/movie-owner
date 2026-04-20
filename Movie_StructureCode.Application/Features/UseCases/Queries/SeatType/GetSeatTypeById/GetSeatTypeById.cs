using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypeById
{
    public static class GetSeatTypeById
    {
        /// <summary>
        /// Query ?? l?y seat type theo ID (User view - thông tin c? b?n)
        /// </summary>
        public sealed record Query(Guid Id) : IQuery<SeatTypeUserDto>, ICacheableQuery
        {
            public bool BypassCache => false;
            public string CacheKey => $"seattype:user:{Id}";
            public Type ValueType => typeof(SeatTypeUserDto);
            public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
        }

        /// <summary>
        /// Query ?? l?y seat type theo ID (Admin view - ??y ?? thông tin)
        /// </summary>
        public sealed record AdminQuery(Guid Id) : IQuery<SeatTypeAdminDto>, ICacheableQuery
        {
            public bool BypassCache => false;
            public string CacheKey => $"seattype:admin:{Id}";
            public Type ValueType => typeof(SeatTypeAdminDto);
            public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
        }

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
