using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovieWithTheaterCount
{
    public sealed class GetShowingsByMovieWithTheaterCountHandler
        : IQueryHandler<GetShowingsByMovieWithTheaterCount.Query, IEnumerable<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IMovieRepository _movieRepo;

        public GetShowingsByMovieWithTheaterCountHandler(
            IShowingRepository showingRepo,
            IMovieRepository movieRepo)
        {
            _showingRepo = showingRepo;
            _movieRepo = movieRepo;
        }

        public async Task<Result<IEnumerable<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>>> Handle(
            GetShowingsByMovieWithTheaterCount.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // Validate: Date must not be in the past
                if (query.Date.Date < DateTime.UtcNow.Date)
                {
                    return Result.Failure<IEnumerable<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>>(
                        new Error("Showing.InvalidDate",
                            $"Date must not be in the past. Requested date: {query.Date:yyyy-MM-dd}, Today: {DateTime.UtcNow:yyyy-MM-dd}"));
                }

                // Verify movie exists
                var movie = await _movieRepo.GetByIdAsync(query.MovieId, cancellationToken);
                if (movie is null)
                {
                    return Result.Failure<IEnumerable<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>>(
                        new Error("Movie.NotFound", $"Movie with ID '{query.MovieId}' not found."));
                }

                // Get theaters with showing count for the movie on the specified date
                var theaterWithCounts = await _showingRepo.CountShowingByMovieWithDateAsync(
                    query.MovieId,
                    query.Date,
                    cancellationToken);

                if (theaterWithCounts == null || !theaterWithCounts.Any())
                {
                    return Result.Success(Enumerable.Empty<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>());
                }

                // Map to DTO - only include Name, Location, and ShowingCount
                var result = theaterWithCounts
                    .Select(tc => new GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto(
                        TheaterName: tc.theater.Name,
                        TheaterLocation: tc.theater.Location,
                        ShowingCount: tc.count))
                    .ToList();

                return Result.Success((IEnumerable<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>)result);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<GetShowingsByMovieWithTheaterCount.TheaterWithShowingCountDto>>(
                    new Error("Showing.Error",
                        $"An error occurred while fetching theaters with showing count: {ex.Message}"));
            }
        }
    }
}
