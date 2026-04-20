using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats
{
    /// <summary>
    /// Handler ?? l?y room kèm seats cho admin
    /// Validation: Ki?m tra room có thu?c v? theater ???c ch? ??nh không
    /// </summary>
    public sealed class GetRoomWithSeatsAdminHandler
        : IQueryHandler<GetRoomWithSeatsAdmin.Query, RoomDetailAdminDto>
    {
        private readonly IRoomRepository _roomRepo;

        public GetRoomWithSeatsAdminHandler(IRoomRepository roomRepo) => _roomRepo = roomRepo;

        public async Task<Result<RoomDetailAdminDto>> Handle(
            GetRoomWithSeatsAdmin.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // STEP 1: L?y room kèm seats t? database
                var room = await _roomRepo.GetRoomWithSeatsAsync(query.RoomId, cancellationToken);

                // STEP 2: Ki?m tra room có tồn tại không
                if (room is null)
                {
                    return Result.Failure<RoomDetailAdminDto>(
                        new Error("Room.NotFound", $"Room with ID '{query.RoomId}' not found."));
                }

                // STEP 3: VALIDATION - Kiểm tra room có thực sự thuộc về theater được chỉ định không
                if (room.TheaterId != query.TheaterId)
                {
                    return Result.Failure<RoomDetailAdminDto>(
                        new Error(
                            "Room.TheaterMismatch",
                            $"Room '{room.Name}' does not belong to Theater '{query.TheaterId}'. " +
                            $"This room actually belongs to Theater '{room.TheaterId}'."));
                }

                // STEP 4: Mapping seats vào layout
                // Lấy danh sách ghế từ room
                var seats = room.Seats ?? new List<Seat>();

                // Group seats by row for Admin
                var layout = room.Seats?
                    .OrderBy(s => s.RowIndex)
                    .ThenBy(s => s.ColIndex)
                    .GroupBy(s => new { s.RowIndex, s.RowName })
                    .Select(rowGroup => new SeatRowLayoutAdminDto(
                        rowGroup.Key.RowIndex,
                        rowGroup.Key.RowName,
                        rowGroup.GroupBy(s => s.GroupId ?? s.Id)
                                .Select(g =>
                                {
                                    var first = g.First();
                                    return new SeatLayoutAdminDto
                                    (
                                        first.GroupId,
                                        g.Count() > 1 ? "Group" : "Single",
                                       first.RowName,
                                        first.RowIndex,
                                         g.Min(s => s.ColIndex),
                                        first.SeatNumber,
                                      g.Select(s => s.Id).ToList(),
                                        first.SeatTypeId,
                                          first.SeatType?.Name ?? "Unknown",
                                        MapConfigStatus(first.StatusSeat)
                                    );
                                })
                                .OrderBy(s => s.StartColIndex)
                                .ToList()
                    ))
                    .ToList();

                var totalSeatUnits = seats
                .Where(s => s.StatusSeat == StatusSeat.Active)
                .GroupBy(s => s.GroupId ?? s.Id)
                .Count();

                // STEP 5: Tạo DTO và trả về
                var dto = new RoomDetailAdminDto(
                    room.Id,
                    room.Name,
                    totalSeatUnits,
                    room.TotalRow,
                    room.TotalCol,
                    room.IsActive,
                    room.DateCreate,
                    room.DateUpdate,
                    layout ?? new List<SeatRowLayoutAdminDto>()
                );

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<RoomDetailAdminDto>(
                    new Error("Room.Error", $"An error occurred while retrieving room: {ex.Message}"));
            }
        }

        private static SeatConfigStatusEnum MapConfigStatus(StatusSeat status)
        {
            return status switch
            {
                StatusSeat.Active => SeatConfigStatusEnum.Active,
                StatusSeat.Broken => SeatConfigStatusEnum.Broken,
                StatusSeat.None => SeatConfigStatusEnum.None,
                _ => SeatConfigStatusEnum.None
            };
        }
    }
}
