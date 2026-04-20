using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatCount
{
    public static class GetSeatCount
    {
        public sealed record Query(
            Guid ShowingId
        ) : IQuery<SeatCountDto>, ICacheableQuery
        {
            // Cache seat counts for 1 hour - they don't change frequently
            public bool BypassCache => false;

            public string CacheKey =>
                $"showingseats:count:{ShowingId}";

            public Type ValueType => typeof(SeatCountDto);
            public TimeSpan? Expiration => TimeSpan.FromHours(1);
        }

        /// <summary>
        /// DTO for seat count statistics
        /// </summary>
        public sealed record SeatCountDto(
            Guid ShowingId,
            int TotalSeats,
            int AvailableSeats,
            int LockedSeats,
            int BookedSeats
        );
    }
}
