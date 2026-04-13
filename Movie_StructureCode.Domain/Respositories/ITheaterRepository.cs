using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface ITheaterRepository : IRepository<Theater>
    {
        /// <summary>
        /// Lấy danh sách rạp có chiếu movie
        /// </summary>
        Task<IEnumerable<Theater>> GetTheatersByMovieAsync(
            Guid movieId,
            DateTime date,
            CancellationToken ct = default);


    }
}
