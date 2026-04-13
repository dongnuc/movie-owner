using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.MarkBrokenSeats
{
    public static class MarkBrokenSeats
    {
        public sealed record Command(
            Guid ShowingId,
            Guid RoomId,
            /// <summary>
            /// Danh sách ID gh? h?ng
            /// </summary>
            IEnumerable<Guid> SeatIds,
            /// <summary>
            /// Lý do (tùy ch?n)
            /// </summary>
            string? Reason = null
        ) : ICommand<MarkBrokenSeatsResult>;
    }

    public sealed record MarkBrokenSeatsResult(
        int MarkedCount,
        int FailedCount,
        IEnumerable<string>? FailedSeatNumbers
    );
}
