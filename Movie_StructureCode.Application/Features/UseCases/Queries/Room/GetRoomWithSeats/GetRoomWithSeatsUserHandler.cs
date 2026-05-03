using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using System.Linq;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats
{
    /// <summary>
    /// Handler ?? l?y room kèm seats cho user
    /// Ch? hi?n th? gh? có StatusSeat = Active (1)
    /// Validation: Ki?m tra showing có t?n t?i không
    /// </summary>
    public sealed class GetRoomWithSeatsUserHandler
        : IQueryHandler<GetRoomWithSeatsUser.Query, RoomDetailUserDto>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly ISeatLockService _seatLockService;
        private readonly IShowingSeatRepository _showingSeatRepo;

        public GetRoomWithSeatsUserHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            ISeatLockService seatLockService,
            IShowingSeatRepository showingSeatRepo)
        {
            _showingRepo = showingRepo;
            _roomRepo = roomRepo;
            _seatLockService = seatLockService;
            _showingSeatRepo = showingSeatRepo;
        }

        public async Task<Result<RoomDetailUserDto>> Handle(
            GetRoomWithSeatsUser.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // STEP 1: Lấy showing từ database
                var showing = await _showingRepo.GetByIdAsync(query.ShowingId, cancellationToken);

                // STEP 2: Kiểm tra showing có tồn tại không
                if (showing is null)
                {
                    return Result.Failure<RoomDetailUserDto>(
                        new Error("Showing.NotFound", $"Showing with ID '{query.ShowingId}' not found."));
                }

                // STEP 3: Lấy room kèm seats từ database
                var room = await _roomRepo.GetRoomWithSeatsAsync(showing.RoomId, cancellationToken);

                // STEP 4: Kiểm tra room có tồn tại không
                if (room is null)
                {
                    return Result.Failure<RoomDetailUserDto>(
                        new Error("Room.NotFound", $"Room with ID '{showing.RoomId}' not found."));
                }

                // STEP 5: Lấy danh sách ghế của suất chiếu (booked, locked, available)
                var showingSeats = await _showingSeatRepo.GetByShowingAsync(showing.Id, cancellationToken);

                // Tạo mapping: SeatId -> ShowingSeat để kiểm tra trạng thái
                var showingSeatMap = showingSeats.ToDictionary(ss => ss.SeatId, ss => ss);

                // STEP 7: Mapping seats vào layout
                // Lấy danh sách ghế từ room - CHỉ những ghế có StatusSeat = Active (1)
                var activeSeats = (room.Seats ?? new List<Seat>())
                    .Where(s => s.StatusSeat == StatusSeat.Active)
                    .ToList();

                // booked
                var bookedSeat = await _showingSeatRepo.GetByShowingAsync(showing.Id, cancellationToken); // booked from database
                var bookedSet = bookedSeat.Select(b => b.SeatId).ToHashSet();

                var lockedSeats = await _seatLockService.GetAllLockedSeatsByShowingAsync(showing.Id); // locked from Redis
                var lockedSet = lockedSeats.ToHashSet();

                var userHoldingSeats = new HashSet<Guid>();
                if (query.UserId != null)
                {
                    userHoldingSeats = await _seatLockService.GetUserHoldingSeatsAsync(showing.Id, query.UserId); // locked của user 
                    //if (userHoldingSeats != null)
                    //{
                    //    lockedSet.UnionWith(userHoldingSeats);
                    //}
                }

                // Group seats by row for User
                var layout = activeSeats
                    .OrderBy(s => s.RowIndex)
                    .ThenBy(s => s.ColIndex)
                    .GroupBy(s => new { s.RowIndex, s.RowName })
                    .Select(rowGroup => new SeatRowLayoutUserDto(
                        rowGroup.Key.RowIndex,
                        rowGroup.Key.RowName,
                        rowGroup.GroupBy(s => s.GroupId ?? s.Id)
                            .Select(g =>
                            {
                                var s = g.First();
                                var status = SeatBookingStatusEnum.Available;
                                var isMine = false;

                                if (bookedSet.Contains(s.Id))
                                {
                                    status = SeatBookingStatusEnum.Booked;
                                }
                                else if (lockedSet.Contains(s.Id))
                                {
                                    status = SeatBookingStatusEnum.Holding;
                                    if (userHoldingSeats != null)
                                    {
                                        isMine = userHoldingSeats!.Contains(s.Id);
                                    }
                                }

                                return new SeatLayoutUserDto(
                                    s.RowIndex,
                                    s.ColIndex,
                                    s.RowName,
                                    s.SeatNumber,
                                    s.SeatTypeId,
                                    s.SeatType?.Name ?? "Unknown",
                                    status,
                                    g.Select(s => s.Id).ToList(),
                                    isMine
                                );
                            })
                            .OrderBy(s => s.ColIndex)
                            .ToList()
                    ))
                    .ToList();

                // STEP 6: Tạo DTO và trả về
                var dto = new RoomDetailUserDto(
                    room.Id,
                    layout ?? new List<SeatRowLayoutUserDto>(),
                    new List<SeatStatusDto>() // ?? trống vì logic này không cần status
                );

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<RoomDetailUserDto>(
                    new Error("Room.Error", $"An error occurred while retrieving room: {ex.Message}"));
            }
        }
    }
}
