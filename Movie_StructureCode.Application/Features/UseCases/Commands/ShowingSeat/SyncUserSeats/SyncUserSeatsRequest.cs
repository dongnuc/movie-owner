using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.SyncUserSeats
{
    public static class SyncUserSeatsRequest
    {
        public sealed record Command(
            Guid ShowingId,
            string UserId,
            IEnumerable<Guid> SelectedSeatIds
        ) : ICommand<SyncUserSeatsResult>;
    }

    public sealed record SyncUserSeatsResult(
        bool Success,
        string Message                   // Operation message
    );

}
