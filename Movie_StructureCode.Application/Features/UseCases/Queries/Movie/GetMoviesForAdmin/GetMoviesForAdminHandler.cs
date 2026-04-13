using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMoviesForAdmin
{
    public sealed class GetMoviesForAdminHandler
        : IQueryHandler<GetMoviesForAdmin.Query, PagedResult<MovieAdminListDto>>
    {
        private readonly IMovieRepository _repo;

        public GetMoviesForAdminHandler(IMovieRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<MovieAdminListDto>>> Handle(
            GetMoviesForAdmin.Query request,
            CancellationToken cancellationToken)
        {
            var paged = await _repo.GetMoviesForAdminAsync(
                request.CategoryId,
                request.Search,
                request.IsActive,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var response = PagedResult<MovieAdminListDto>.Create(
                paged.Items.Select(GetMoviesForAdmin.ToDto).ToList(),
                paged.PageIndex,
                paged.PageSize,
                paged.TotalCount);

            return Result.Success(response);
        }
    }
}
