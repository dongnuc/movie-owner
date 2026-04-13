using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Seat.CreateSeatsForRoom
{
    public sealed class CreateSeatsForRoomHandler
        : ICommandHander<CreateSeatsForRoom.Command, CreateSeatsForRoomResult>
    {
        private readonly IRoomRepository _roomRepo;
        private readonly ISeatRepository _seatRepo;
        private readonly IUnitOfWork _uow;

        public CreateSeatsForRoomHandler(
            IRoomRepository roomRepo,
            ISeatRepository seatRepo,
            IUnitOfWork uow)
        {
            _roomRepo = roomRepo;
            _seatRepo = seatRepo;
            _uow = uow;
        }

        public async Task<Result<CreateSeatsForRoomResult>> Handle(
            CreateSeatsForRoom.Command command,
            CancellationToken cancellationToken)
        {
            // Verify room exists
            var room = await _roomRepo.GetByIdAsync(command.RoomId, cancellationToken);
            if (room is null)
                return Result.Failure<CreateSeatsForRoomResult>(
                    new Error("Room.NotFound", $"Room '{command.RoomId}' not found."));

            // Verify dimensions match room
            if (command.TotalRow != room.TotalRow || command.TotalCol != room.TotalCol)
                return Result.Failure<CreateSeatsForRoomResult>(
                    new Error("Room.DimensionMismatch",
                        $"Seat dimensions ({command.TotalRow}×{command.TotalCol}) do not match room dimensions ({room.TotalRow}×{room.TotalCol})."));

            // Create seats
            var seats = new List<Domain.Entities.Seat>();
            var createdSeatIds = new List<Guid>();

            for (int row = 0; row < command.TotalRow; row++)
            {
                for (int col = 0; col < command.TotalCol; col++)
                {
                    var seatId = Guid.NewGuid();

                    var seat = new Domain.Entities.Seat
                    {
                        Id = seatId,
                        RoomId = command.RoomId,
                        RowIndex = row,
                        ColIndex = col,
                        RowName = GetRowName(row),      // A, B, C, ...
                        SeatNumber = col + 1,            // 1, 2, 3, ...
                        SeatTypeId = command.DefaultSeatTypeId ?? Guid.Empty,
                        StatusSeat = StatusSeat.Active,
                        IsActive = true,
                        DateCreate = DateTime.UtcNow,
                        DateUpdate = DateTime.UtcNow
                    };

                    seats.Add(seat);
                    createdSeatIds.Add(seatId);
                }
            }

            // Add all seats to repository
            foreach (var seat in seats)
            {
                await _seatRepo.AddAsync(seat, cancellationToken);
            }

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateSeatsForRoomResult(seats.Count, createdSeatIds));
        }

        /// <summary>
        /// Convert row index to row name (0 -> A, 1 -> B, 2 -> C, ...)
        /// </summary>
        private static string GetRowName(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > 25)
                return $"R{rowIndex}";  // Fallback for rows beyond Z

            return ((char)('A' + rowIndex)).ToString();
        }
    }
}
