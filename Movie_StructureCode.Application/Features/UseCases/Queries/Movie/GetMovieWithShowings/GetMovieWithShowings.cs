using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieWithShowings
{
    public static class GetMovieWithShowings
    {
        public sealed record Query(Guid MovieId) : IQuery<MovieWithShowingsUserDto>;
    }
}
