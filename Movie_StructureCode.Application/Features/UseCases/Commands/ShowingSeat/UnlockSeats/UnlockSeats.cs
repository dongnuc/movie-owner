using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.UnlockSeats
{
    public static class UnlockSeats
    {
        public sealed record Command(
            Guid ShowingId,
            Guid RoomId,
            /// <summary>
            /// Danh sách ID gh? c?n unlock
            /// </summary>
            IEnumerable<Guid> SeatIds
        ) : ICommand<UnlockSeatsResult>;
    }

    public sealed record UnlockSeatsResult(
        int UnlockedCount,
        int FailedCount,
        IEnumerable<string>? FailedSeatNumbers
    );
}
