using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByDateRange
{
    public static class GetShowingsByDateRange
    {
        public sealed record Query(
            Guid MovieId,
            DateTime Date
        ) : IQuery<IEnumerable<ShowingsSummaryDto>>;
    }
}
