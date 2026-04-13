using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMoviesForAdmin
{
    public static class GetMoviesForAdmin
    {
        public sealed record Query(
            Guid?   CategoryId,
            string? Search,
            bool?   IsActive,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<MovieAdminListDto>>;

        /// <summary>
        /// Map Entity ? Admin List DTO (compact view for table - essential fields only)
        /// </summary>
        public static MovieAdminListDto ToDto(Domain.Entities.Movie m) =>
            new(m.Id, m.Title, m.BasePrice, m.Duration,
                m.Category?.Name, m.IsActive);
    }
}
