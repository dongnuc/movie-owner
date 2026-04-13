using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.LockIndividualSeats
{
    public static class LockIndividualSeats
    {
        public sealed record Command(
            Guid ShowingId,
            Guid RoomId,
            /// <summary>
            /// Danh sách ID c?a gh? c?n lock
            /// </summary>
            IEnumerable<Guid> SeatIds,
            /// <summary>
            /// Lý do lock (h?ng, b?o trì, VIP riêng, v.v.)
            /// </summary>
            string? Reason = null
        ) : ICommand<LockIndividualSeatsResult>;
    }

    public sealed record LockIndividualSeatsResult(
        int LockedCount,
        int FailedCount,
        DateTime LockedUntil,
        IEnumerable<string>? FailedSeatNumbers
    );
}
