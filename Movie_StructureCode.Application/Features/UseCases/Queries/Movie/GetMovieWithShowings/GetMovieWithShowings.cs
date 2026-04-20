using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieWithShowings
{
    public static class GetMovieWithShowings
    {
        public sealed record Query(Guid MovieId) : IQuery<MovieWithShowingsUserDto>, ICacheableQuery
        {
            public bool BypassCache => false;
            public string CacheKey => $"movie:showings:{MovieId}";
            public Type ValueType => typeof(MovieWithShowingsUserDto);
            public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
        }
    }
}
