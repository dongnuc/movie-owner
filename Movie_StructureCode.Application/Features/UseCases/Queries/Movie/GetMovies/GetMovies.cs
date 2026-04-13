using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovies
{
    public static class GetMovies
    {
        public sealed record Query(
            Guid?   CategoryId,
            string? Search,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<MovieUserDto>>;

        /// <summary>
        /// Map Entity ? User DTO (minimal fields for user display)
        /// </summary>
        public static MovieUserDto ToDtoUser(Domain.Entities.Movie m) =>
            new(m.Id, m.Title, m.Description, m.Image,
                m.BasePrice, m.Duration, m.Category?.Name);

        /// <summary>
        /// Map Entity ? Admin DTO (full fields for admin management)
        /// </summary>
        public static MovieAdminDto ToDtoAdmin(Domain.Entities.Movie m) =>
            new(m.Id, m.Title, m.Description, m.Image,
                m.BasePrice, m.Duration,
                m.CategoryId, m.Category?.Name,
                m.IsActive, m.DateCreate, m.DateUpdate);

        /// <summary>
        /// Legacy mapper - dùng chung (deprecated)
        /// </summary>
        public static MovieDto ToDto(Domain.Entities.Movie m) =>
            new(m.Id, m.Title, m.Description, m.Image,
                m.BasePrice, m.Duration,
                m.CategoryId, m.Category?.Name,
                m.IsActive, m.DateCreate, m.DateUpdate);
    }
}
