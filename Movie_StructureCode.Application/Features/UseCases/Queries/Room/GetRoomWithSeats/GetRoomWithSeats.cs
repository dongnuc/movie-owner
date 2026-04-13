using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats
{
    public static class GetRoomWithSeats
    {
        public sealed record Query(Guid RoomId) : IQuery<RoomWithSeatsUserDto>;
    }
}
