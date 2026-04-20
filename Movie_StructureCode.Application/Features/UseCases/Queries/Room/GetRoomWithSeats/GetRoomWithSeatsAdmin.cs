using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats
{
    public static class GetRoomWithSeatsAdmin
    {
        /// <summary>
        /// Query ?? l?y room kèm seat layout cho admin
        /// B?t bu?c: TheaterId và RoomId
        /// Validation: Room ph?i thu?c v? Theater ???c ch? ??nh, n?u không s? báo l?i
        /// </summary>
        public sealed record Query(
            Guid TheaterId,
            Guid RoomId
        ) : IQuery<RoomDetailAdminDto>;
    }
}
