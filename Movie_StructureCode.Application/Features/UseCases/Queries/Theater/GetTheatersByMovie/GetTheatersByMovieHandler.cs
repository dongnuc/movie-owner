using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheatersByMovie
{
    public sealed class GetTheatersByMovieHandler
        : IQueryHandler<GetTheatersByMovie.Query, IEnumerable<TheaterDto>>
    {
        private readonly ITheaterRepository _repo;

        public GetTheatersByMovieHandler(ITheaterRepository repo) => _repo = repo;

        public async Task<Result<IEnumerable<TheaterDto>>> Handle(
            GetTheatersByMovie.Query query,
            CancellationToken        cancellationToken)
        {
            if (query.Date < DateTime.UtcNow)
            {
                return Result.Failure<IEnumerable<TheaterDto>>(
                    new Error("InvalidDate", "Date must be in the future."));

            }

            var theaters = await _repo.GetTheatersByMovieAsync(
                query.MovieId,
                query.Date,
                cancellationToken);

            return Result.Success(theaters.Select(GetTheatersByMovie.ToDto));
        }
    }
}
