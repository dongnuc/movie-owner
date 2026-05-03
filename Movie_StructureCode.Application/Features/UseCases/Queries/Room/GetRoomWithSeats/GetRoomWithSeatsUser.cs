using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats
{
    public static class GetRoomWithSeatsUser
    {
        /// <summary>
        /// Query ?? l?y room kèm seat layout cho user
        /// B?t bu?c: ShowingId
        /// Logic: Ch? hi?n th? gh? có StatusSeat = Active (1)
        /// Validation: Showing ph?i t?n t?i
        /// </summary>
        public sealed record Query(
            Guid ShowingId,
            string? UserId
        ) : IQuery<RoomDetailUserDto>;
    }
}
