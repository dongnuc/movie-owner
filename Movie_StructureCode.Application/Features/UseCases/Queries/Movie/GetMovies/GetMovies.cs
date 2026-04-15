using Movie_StructureCode.Application.Common;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovies
{
    public static class GetMovies
    {
        public sealed record Query(
            Guid? CategoryId,
            string? Search,
            int PageNumber,
            int PageSize
        ) : IQuery<PagedResult<MovieUserDto>>, ICacheableQuery
        {
            // nocache if search or pagination is used, otherwise cache the first page of all movies
            public bool BypassCache =>
               !string.IsNullOrWhiteSpace(Search) || PageNumber > 1;

            public string CacheKey
            {
                get
                {
                    var categoryPart = CategoryId?.ToString() ?? "all";

                    return $"MovieApp:movies:category:{categoryPart}:page:{PageNumber}:size:{PageSize}";
                }
            }
            public Type ValueType => typeof(PagedResult<MovieUserDto>);
            public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
        };

        /// <summary>
        /// Map Entity ? User DTO (minimal fields for user display)
        /// </summary>
        public static MovieUserDto ToDtoUser(Domain.Entities.Movie m) =>
            new(m.Id, m.Title, m.Description, m.Image,
                m.BasePrice, m.Duration, m.Category?.Name);

        
    }
}
