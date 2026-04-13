using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByDateRange
{
    public sealed class GetShowingsByDateRangeHandler
        : IQueryHandler<GetShowingsByDateRange.Query, IEnumerable<ShowingsSummaryDto>>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IMovieRepository _movieRepo;

        public GetShowingsByDateRangeHandler(IShowingRepository showingRepo, IMovieRepository movieRepo)
        {
            _showingRepo = showingRepo;
            _movieRepo = movieRepo;
        }

        public async Task<Result<IEnumerable<ShowingsSummaryDto>>> Handle(
            GetShowingsByDateRange.Query query,
            CancellationToken cancellationToken)
        {
            // Verify movie exists
            var movie = await _movieRepo.GetByIdAsync(query.MovieId, cancellationToken);
            if (movie is null)
                return Result.Failure<IEnumerable<ShowingsSummaryDto>>(
                    new Error("Movie.NotFound", $"Movie '{query.MovieId}' not found."));

            // Get all showings for this movie
            var allShowings = await _showingRepo.GetByMovieAsync(query.MovieId, cancellationToken);

            // Filter by the specified date (from 00:00 to 23:59:59)
            var dateStart = query.Date.Date;
            var dateEnd = dateStart.AddDays(1);

            var showings = allShowings
                .Where(s => s.TimeStart >= dateStart && s.TimeStart < dateEnd)
                .ToList();

            // Group by Theater and Room
            var summary = showings
                .GroupBy(s => new { s.Room?.TheaterId, s.Room?.Theater?.Name, s.Room?.Theater?.Location })
                .SelectMany(theaterGroup =>
                    theaterGroup
                        .GroupBy(s => new { s.RoomId, s.Room?.Name })
                        .Select(roomGroup => new ShowingsSummaryDto(
                            theaterGroup.Key.TheaterId ?? Guid.Empty,
                            theaterGroup.Key.Name ?? "Unknown",
                            theaterGroup.Key.Location,
                            roomGroup.Key.RoomId,
                            roomGroup.Key.Name ?? "Unknown",
                            roomGroup.Count(),
                            roomGroup
                                .Select(s => new ShowingTimeDto(
                                    s.Id,
                                    s.TimeStart,
                                    s.Price))
                                .OrderBy(dto => dto.TimeStart)
                                .ToList())))
                .ToList();

            return Result.Success(summary.AsEnumerable());
        }
    }
}
