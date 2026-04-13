using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheatersByMovie
{
    public static class GetTheatersByMovie
    {
        public sealed record Query(
            Guid     MovieId,
            DateTime Date
        ) : IQuery<IEnumerable<TheaterDto>>;

        /// <summary>
        /// Map Entity ? DTO
        /// </summary>
        public static TheaterDto ToDto(Domain.Entities.Theater t) =>
            new(t.Id, t.Name, t.Location, t.IsActive, t.DateCreate);
    }
}
