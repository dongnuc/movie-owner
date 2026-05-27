using Movie_StructureCode.Application.Features.UseCases.Queries.Movie;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieByTheaterId
{
    /// <summary>
    /// Handler xử lý query lấy danh sách phim đang chiếu tại một rạp chiếu phim
    /// Sử dụng TheaterId denormalized từ Showing entity để tối ưu query performance
    /// </summary>
    public sealed class GetMovieByTheaterIdHandler
        : IQueryHandler<GetMovieByTheaterId.Query, PagedResult<MovieListByTheaterId>>
    {
        private readonly IMovieRepository _movieRepo;

        public GetMovieByTheaterIdHandler(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        public async Task<Result<PagedResult<MovieListByTheaterId>>> Handle(
            GetMovieByTheaterId.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // STEP 1: Validate input
                if (query.TheaterId == Guid.Empty)
                {
                    return Result.Failure<PagedResult<MovieListByTheaterId>>(
                        new Error("Theater.Invalid", "Theater ID cannot be empty."));
                }

                if (query.PageNumber < 1)
                {
                    return Result.Failure<PagedResult<MovieListByTheaterId>>(
                        new Error("Pagination.Invalid", "Page number must be greater than 0."));
                }

                if (query.PageSize < 1 || query.PageSize > 100)
                {
                    return Result.Failure<PagedResult<MovieListByTheaterId>>(
                        new Error("Pagination.Invalid", "Page size must be between 1 and 100."));
                }

                // STEP 2: Lấy danh sách phim từ repository
                // Query đã được tối ưu với index trên (TheaterId, IsActive, MovieId)
                var moviesPagedResult = await _movieRepo.GetMoviesByTheaterIdAsync(
                    query.TheaterId,
                    query.PageNumber,
                    query.PageSize,
                    cancellationToken);

                // STEP 3: Kiểm tra kết quả
                if (!moviesPagedResult.Items.Any())
                {
                    return Result.Success(PagedResult<MovieListByTheaterId>.Create(
                        new List<MovieListByTheaterId>(),
                        query.PageNumber,
                        query.PageSize,
                        0));
                }

                // STEP 4: Mapping entities sang DTO
                var dtos = moviesPagedResult.Items
                    .Select(movie => GetMovieByTheaterId.ToListDto(movie))
                    .ToList();

                // STEP 5: Tạo result
                var result = PagedResult<MovieListByTheaterId>.Create(
                    dtos,
                    query.PageNumber,
                    query.PageSize,
                    moviesPagedResult.TotalCount);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<PagedResult<MovieListByTheaterId>>(
                    new Error("Movie.Error",
                        $"An error occurred while fetching movies by theater: {ex.Message}"));
            }
        }
    }
}
