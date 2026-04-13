using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.CreateShowingSeats
{
    public static class CreateShowingSeats
    {
        public sealed record Command(
            Guid ShowingId,
            Guid RoomId,
            decimal Price,
            /// <summary>
            /// Danh sách hàng gh? c?n lock (0-based index). Null = không lock hàng nào
            /// </summary>
            IEnumerable<int>? LockedRowIndices = null
        ) : ICommand<CreateShowingSeatsResult>;
    }

    public sealed record CreateShowingSeatsResult(
        int TotalCreated,
        int TotalLocked,
        IEnumerable<Guid> CreatedShowingSeatIds
    );
}
