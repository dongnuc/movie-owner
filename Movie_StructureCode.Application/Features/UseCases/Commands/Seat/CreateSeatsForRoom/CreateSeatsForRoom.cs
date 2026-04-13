using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Seat.CreateSeatsForRoom
{
    public static class CreateSeatsForRoom
    {
        public sealed record Command(
            Guid    RoomId,
            int     TotalRow,
            int     TotalCol,
            /// <summary>
            /// Default SeatTypeId for all seats (optional).
            /// If not provided, seats will have empty SeatTypeId.
            /// </summary>
            Guid?   DefaultSeatTypeId = null
        ) : ICommand<CreateSeatsForRoomResult>;
    }

    public sealed record CreateSeatsForRoomResult(
        int TotalCreated,
        IEnumerable<Guid> CreatedSeatIds
    );
}
