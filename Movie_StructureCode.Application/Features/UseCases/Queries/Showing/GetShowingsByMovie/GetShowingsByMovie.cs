using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovie
{
    public static class GetShowingsByMovie
    {
        public sealed record Query(
            Guid MovieId,
            Guid? TheaterId = null,
            DateTime? Date = null
        ) : IQuery<IEnumerable<ShowingDto>>;
    }
}
