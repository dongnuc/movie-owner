using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieById
{
    public static class GetMovieById
    {
        /// <summary>
        /// Query ?? l?y movie theo ID (User view - thông tin c? b?n)
        /// </summary>
        public sealed record Query(Guid Id) : IQuery<MovieUserDto>, ICacheableQuery
        {
            public bool BypassCache => false;
            public string CacheKey => $"movie:user:{Id}";
            public Type ValueType => typeof(MovieUserDto);
            public TimeSpan? Expiration => TimeSpan.FromHours(1);
        }

        /// <summary>
        /// Query ?? l?y movie theo ID (Admin view - ??y ?? thông tin)
        /// </summary>
        public sealed record AdminQuery(Guid Id) : IQuery<MovieAdminDto>, ICacheableQuery
        {
            public bool BypassCache => false;
            public string CacheKey => $"movie:admin:{Id}";
            public Type ValueType => typeof(MovieAdminDto);
            public TimeSpan? Expiration => TimeSpan.FromHours(1);
        }

        /// <summary>
        /// Map to User DTO (thông tin c? b?n)
        /// </summary>
        public static MovieUserDto ToDtoUser(Domain.Entities.Movie m) =>
            GetMovies.GetMovies.ToDtoUser(m);

        /// <summary>
        /// Map to Admin DTO (??y ?? thông tin qu?n lý)
        /// </summary>
        public static MovieAdminDto ToDtoAdmin(Domain.Entities.Movie m) =>
            new(
                m.Id,
                m.Title,
                m.Description,
                m.Image,
                m.BasePrice,
                m.Duration,
                m.CategoryId,
                m.Category?.Name,
                m.IsActive,
                m.DateCreate,
                m.DateUpdate
            );
    }
}
