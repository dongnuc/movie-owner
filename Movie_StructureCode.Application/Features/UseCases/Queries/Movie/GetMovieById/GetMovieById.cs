using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieById
{
    public static class GetMovieById
    {
        public sealed record Query(Guid Id) : IQuery<MovieUserDto>;

        /// <summary>
        /// Map to User DTO
        /// </summary>
        public static MovieUserDto ToDtoUser(Domain.Entities.Movie m) =>
            GetMovies.GetMovies.ToDtoUser(m);

        /// <summary>
        /// Map to Admin DTO
        /// </summary>
        public static MovieAdminDto ToDtoAdmin(Domain.Entities.Movie m) =>
            GetMovies.GetMovies.ToDtoAdmin(m);

        /// <summary>
        /// Legacy mapper - deprecated
        /// </summary>
        public static MovieDto ToDto(Domain.Entities.Movie m) =>
            GetMovies.GetMovies.ToDto(m);
    }
}
