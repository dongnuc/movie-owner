using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatMap
{
    public static class GetSeatMap
    {
        public sealed record Query(
            Guid ShowingId,
            Guid RoomId
        ) : IQuery<SeatMapDto>;
    }
}
