using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Seat.CreateSeatsForRoom
{
    public static class CreateSeatsForRoom
    {
        public sealed record Command(
            Guid    RoomId,
            int     TotalRow,
            int     TotalCol
        ) : ICommand<CreateSeatsForRoomResult>;
    }

    public sealed record CreateSeatsForRoomResult(
        int TotalCreated
    );
}
