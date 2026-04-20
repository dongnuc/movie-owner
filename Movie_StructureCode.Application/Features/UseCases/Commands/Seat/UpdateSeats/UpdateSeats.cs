using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Features.UseCases.Commands.Seat;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Seat.UpdateSeats
{
    public static class UpdateSeats
    {
        /// <summary>
        /// Command ?? batch update seats
        /// Nh?n vào m?ng seats c?n update v?i các params: Id, SeatTypeId, StatusSeat, GroupId
        /// X? lý trên memory và ch? save 1 l?n
        /// Return: S? l??ng update thành công, th?t b?i
        /// </summary>
        public sealed record Command(
            IEnumerable<UpdateSeatDto> Seats
        ) : ICommand<BatchUpdateSeatsResultDto>;
    }
}
