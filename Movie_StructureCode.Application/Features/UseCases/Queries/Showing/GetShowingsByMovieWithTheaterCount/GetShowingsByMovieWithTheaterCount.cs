using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovieWithTheaterCount
{
    public static class GetShowingsByMovieWithTheaterCount
    {
        public sealed record Query(
            Guid MovieId,
            DateTime Date
        ) : IQuery<IEnumerable<TheaterWithShowingCountDto>>, ICacheableQuery
        {
            // Bypass cache if date is in the past
            public bool BypassCache => Date.Date < DateTime.UtcNow.Date;

            public string CacheKey =>
                $"showings:movie:theaters:{MovieId}:date:{Date:yyyy-MM-dd}";

            public Type ValueType => typeof(IEnumerable<TheaterWithShowingCountDto>);
            public TimeSpan? Expiration => TimeSpan.FromHours(2);
        }

        /// <summary>
        /// DTO for theater with showing count - includes only Name, Location, and count
        /// </summary>
        public sealed record TheaterWithShowingCountDto(
            string  TheaterName,
            string? TheaterLocation,
            int     ShowingCount
        );
    }
}
