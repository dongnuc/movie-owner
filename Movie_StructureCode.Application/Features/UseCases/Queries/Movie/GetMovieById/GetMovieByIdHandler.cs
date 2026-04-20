using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieById
{
    /// <summary>
    /// Handler ?? x? lý query l?y movie theo ID cho User
    /// </summary>
    public sealed class GetMovieByIdHandler
        : IQueryHandler<GetMovieById.Query, MovieUserDto>
    {
        private readonly IMovieRepository _repo;

        public GetMovieByIdHandler(IMovieRepository repo) => _repo = repo;

        public async Task<Result<MovieUserDto>> Handle(
            GetMovieById.Query query,
            CancellationToken  cancellationToken)
        {
            var movie = await _repo.GetByIdWithCategoryAsync(query.Id, cancellationToken);

            if (movie is null)
                return Result.Failure<MovieUserDto>(
                    new Error("Movie.NotFound", $"Movie '{query.Id}' not found."));

            return Result.Success(GetMovieById.ToDtoUser(movie));
        }
    }

    /// <summary>
    /// Handler ?? x? lý query l?y movie theo ID cho Admin (??y ?? thông tin)
    /// </summary>
    public sealed class GetMovieByIdAdminHandler
        : IQueryHandler<GetMovieById.AdminQuery, MovieAdminDto>
    {
        private readonly IMovieRepository _repo;

        public GetMovieByIdAdminHandler(IMovieRepository repo) => _repo = repo;

        /// <summary>
        /// X? lý query l?y movie theo ID cho admin
        /// </summary>
        public async Task<Result<MovieAdminDto>> Handle(
            GetMovieById.AdminQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                // L?y movie t? database theo ID kèm category
                var movie = await _repo.GetByIdWithCategoryAsync(query.Id, cancellationToken);

                // N?u movie không t?n t?i, tr? v? l?i
                if (movie is null)
                {
                    return Result.Failure<MovieAdminDto>(
                        new Error("Movie.NotFound", $"Movie with ID '{query.Id}' not found."));
                }

                // Mapping entity sang Admin DTO (??y ??: Id, Title, Description, Image, BasePrice, Duration, CategoryId, CategoryName, IsActive, DateCreate, DateUpdate)
                var dto = GetMovieById.ToDtoAdmin(movie);

                // Tr? v? k?t qu? thành công
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                // X? lý exception
                return Result.Failure<MovieAdminDto>(
                    new Error("Movie.Error", $"An error occurred while retrieving movie: {ex.Message}"));
            }
        }
    }
}
