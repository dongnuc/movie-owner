using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieWithShowings
{
    public sealed class GetMovieWithShowingsHandler
        : IQueryHandler<GetMovieWithShowings.Query, MovieWithShowingsUserDto>
    {
        private readonly IMovieRepository _repo;

        public GetMovieWithShowingsHandler(IMovieRepository repo) => _repo = repo;

        public async Task<Result<MovieWithShowingsUserDto>> Handle(
            GetMovieWithShowings.Query request,
            CancellationToken cancellationToken)
        {
            var movie = await _repo.GetMovieWithShowingsAsync(request.MovieId, cancellationToken);

            if (movie is null)
                return Result.Failure<MovieWithShowingsUserDto>(
                    new Error("Movie.NotFound", $"Movie '{request.MovieId}' not found."));

            // Filter only active showings for user
            var showingDtos = movie.Showings?
                .Where(s => s.IsActive)
                .Select(s => new ShowingUserDto(
                    s.Id,
                    s.TimeStart,
                    s.Price,
                    s.MaxTicketsPerUser,
                    s.Room?.Name))
                .ToList() ?? new List<ShowingUserDto>();

            var dto = new MovieWithShowingsUserDto(
                movie.Id,
                movie.Title,
                movie.Description,
                movie.Image,
                movie.BasePrice,
                movie.Duration,
                movie.Category?.Name,
                showingDtos);

            return Result.Success(dto);
        }
    }
}
