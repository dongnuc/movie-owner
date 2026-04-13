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

            if (query.TheaterId.HasValue)
            {
                // L?y showings theo movie + theater + ngày (optional)
                showings = await _repo.GetShowingsActiveAsync(
                    query.MovieId,
                    query.TheaterId.Value,
                    query.Date,
                    cancellationToken);
            }
            else
            {
                // L?y t?t c? showings c?a movie
                showings = await _repo.GetByMovieAsync(query.MovieId, cancellationToken);

                if (query.Date.HasValue)
                {
                    var from = query.Date.Value.Date;
                    var to = from.AddDays(1);
                    showings = showings.Where(s => s.TimeStart >= from && s.TimeStart < to);
                }
            }

            var dtos = showings
                .Select(s => new ShowingDto(
                    s.Id,
                    s.TimeStart,
                    s.Price,
                    s.Movie?.Title,
                    s.Room?.Name))
                .ToList();

            return Result.Success(dtos.AsEnumerable());
        }
    }
}
