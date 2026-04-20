using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovie
{
    public sealed class GetShowingsByMovieHandler
        : IQueryHandler<GetShowingsByMovie.Query, IEnumerable<ShowingDto>>
    {
        private readonly IShowingRepository _repo;

        public GetShowingsByMovieHandler(IShowingRepository repo) => _repo = repo;

        public async Task<Result<IEnumerable<ShowingDto>>> Handle(
            GetShowingsByMovie.Query query,
            CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Showing> showings;

            showings = await _repo.GetShowingsActiveAsync(
                query.MovieId,
                query.TheaterId,
                query.Date,
                cancellationToken);

            var dtos = showings
                .Select(s => new ShowingDto(
                    s.Id,
                    s.TimeStart,
                    s.Price,
                    s.Room!.TotalSeat))
                .ToList();

            return Result.Success(dtos.AsEnumerable());
        }
    }
}
