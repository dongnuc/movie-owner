using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats
{
    public sealed class GetRoomWithSeatsHandler
        : IQueryHandler<GetRoomWithSeats.Query, RoomWithSeatsUserDto>
    {
        private readonly IRoomRepository _roomRepo;

        public GetRoomWithSeatsHandler(IRoomRepository roomRepo) => _roomRepo = roomRepo;

        public async Task<Result<RoomWithSeatsUserDto>> Handle(
            GetRoomWithSeats.Query query,
            CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetRoomWithSeatsAsync(query.RoomId, cancellationToken);

            if (room is null)
                return Result.Failure<RoomWithSeatsUserDto>(
                    new Error("Room.NotFound", $"Room '{query.RoomId}' not found."));

            // Group seats by row for User (only Active seats)
            var seatsByRow = room.Seats?
                .Where(s => s.StatusSeat.ToString() == "Active")
                .OrderBy(s => s.RowIndex)
                .ThenBy(s => s.ColIndex)
                .GroupBy(s => new { s.RowIndex, s.RowName })
                .Select(rowGroup => new SeatRowLayoutUserDto(
                    rowGroup.Key.RowIndex,
                    rowGroup.Key.RowName,
                    rowGroup
                        .Select(s => new SeatLayoutUserDto(
                            s.Id,
                            s.RowIndex,
                            s.ColIndex,
                            s.RowName,
                            s.SeatNumber,
                            s.SeatType?.Name,
                            s.SeatType?.Surcharge ?? 0,
                            s.StatusSeat.ToString()))
                        .ToList()))
                .ToList() ?? new List<SeatRowLayoutUserDto>();

            var dto = new RoomWithSeatsUserDto(
                room.Id,
                room.Name,
                room.TotalSeat,
                room.TotalRow,
                room.TotalCol,
                room.Theater?.Name,
                seatsByRow);

            return Result.Success(dto);
        }
    }
}
