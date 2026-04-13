using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.LockSeats
{
    public static class LockSeats
    {
        public sealed record Command(
            Guid ShowingId,
            Guid RoomId,
            /// <summary>
            /// Danh sách hàng gh? c?n lock (0-based index)
            /// </summary>
            IEnumerable<int> RowIndices
        ) : ICommand<LockSeatsResult>;
    }

    public sealed record LockSeatsResult(
        int LockedCount,
        DateTime LockedUntil
    );
}
