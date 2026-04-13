using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieById
{
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
}
