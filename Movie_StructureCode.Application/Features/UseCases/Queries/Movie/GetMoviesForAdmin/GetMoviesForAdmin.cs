using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMoviesForAdmin
{
    public static class GetMoviesForAdmin
    {
        public sealed record Query(
            Guid? CategoryId,
            string? Search,
            bool? IsActive,
            int PageNumber,
            int PageSize
        ) : IQuery<PagedResult<MovieAdminListDto>>, ICacheableQuery
        {
            // Bypass cache when search is used or when filtering by IsActive
            // Only cache the unfiltered default view for first page
            public bool BypassCache =>
                !string.IsNullOrWhiteSpace(Search) ||
                IsActive.HasValue ||
                PageSize != 10 ||
                PageNumber > 1;

            public string CacheKey
            {
                get
                {
                    var categoryPart = CategoryId?.ToString() ?? "all";
                    return $"movie:admin:list:category:{categoryPart}:page:1";
                }
            }

            public Type ValueType => typeof(PagedResult<MovieAdminListDto>);
            public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
        };

        /// <summary>
        /// Map Entity ? Admin List DTO (compact view for table - essential fields only)
        /// </summary>
        public static MovieAdminListDto ToDto(Domain.Entities.Movie m) =>
            new(m.Id, m.Title, m.BasePrice, m.Duration,
                m.Category?.Name, m.IsActive);
    }
}
