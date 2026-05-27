using Movie_StructureCode.Application.Features.UseCases.Queries.Movie;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieByTheaterId
{
    public static class GetMovieByTheaterId
    {
        /// <summary>
        /// Query lấy danh sách phim đang chiếu tại một rạp chiếu phim
        /// TheaterId là bắt buộc - không thể null
        /// Sử dụng denormalized TheaterId từ Showing entity để tối ưu query
        /// </summary>
        public sealed record Query(
            Guid    TheaterId,      // Bắt buộc - không thể null
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<MovieListByTheaterId>>;

        /// <summary>
        /// Map Entity → User List DTO (compact view - chỉ thông tin cơ bản)
        /// Không load Showings relationships để giữ query performance cao
        /// </summary>
        public static MovieListByTheaterId ToListDto(Domain.Entities.Movie movie) =>
            new(
                movie.Id,
                movie.Title,
                movie.Image,
                movie.BasePrice,
                movie.Duration,
                movie.Category?.Name);
    }
}
