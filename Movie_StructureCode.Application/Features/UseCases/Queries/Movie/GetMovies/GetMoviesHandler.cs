using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovies
{
    public sealed class GetMoviesHandler
        : IQueryHandler<GetMovies.Query, PagedResult<MovieUserDto>>
    {
        private readonly IMovieRepository _repo;

        public GetMoviesHandler(IMovieRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<MovieUserDto>>> Handle(
            GetMovies.Query   request,
            CancellationToken cancellationToken)
        {
            var paged = await _repo.GetMoviesForUserAsync(
                request.CategoryId,
                request.Search,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var response = PagedResult<MovieUserDto>.Create(
                paged.Items.Select(GetMovies.ToDtoUser).ToList(),
                paged.PageIndex,
                paged.PageSize,
                paged.TotalCount);

            return Result.Success(response);
        }
    }
}
