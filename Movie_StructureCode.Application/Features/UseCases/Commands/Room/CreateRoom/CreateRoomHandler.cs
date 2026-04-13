using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using MediatR;
using Movie_StructureCode.Application.Features.UseCases.Commands.Seat.CreateSeatsForRoom;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Room.CreateRoom
{
    public sealed class CreateRoomHandler
        : ICommandHander<CreateRoom.Command, Guid>
    {
        private readonly ITheaterRepository _theaterRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IUnitOfWork _uow;
        private readonly ISender _sender;

        public CreateRoomHandler(
            ITheaterRepository theaterRepo,
            IRoomRepository roomRepo,
            IUnitOfWork uow,
            ISender sender)
        {
            _theaterRepo = theaterRepo;
            _roomRepo = roomRepo;
            _uow = uow;
            _sender = sender;
        }

        public async Task<Result<Guid>> Handle(
            CreateRoom.Command command,
            CancellationToken cancellationToken)
        {
            // Verify theater exists
            var theater = await _theaterRepo.GetByIdAsync(command.TheaterId, cancellationToken);
            if (theater is null)
                return Result.Failure<Guid>(
                    new Error("Theater.NotFound", $"Theater '{command.TheaterId}' not found."));

            // Verify room name is unique in theater
            var existingRoom = await _roomRepo.AnyAsync(
                r => r.TheaterId == command.TheaterId && r.Name == command.Name,
                cancellationToken);

            if (existingRoom)
                return Result.Failure<Guid>(
                    new Error("Room.DuplicateName",
                        $"Room name '{command.Name}' already exists in theater '{command.TheaterId}'."));

            // Calculate total seats
            int totalSeat = command.TotalRow * command.TotalCol;

            // Create room
            var room = new Domain.Entities.Room
            {
                Id = Guid.NewGuid(),
                TheaterId = command.TheaterId,
                Name = command.Name,
                TotalRow = command.TotalRow,
                TotalCol = command.TotalCol,
                TotalSeat = totalSeat,
                IsActive = true,
                DateCreate = DateTime.UtcNow,
                DateUpdate = DateTime.UtcNow
            };

            await _roomRepo.AddAsync(room, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            // Create seats using CreateSeatsForRoom usecase
            var createSeatsCommand = new CreateSeatsForRoom.Command(
                room.Id,
                command.TotalRow,
                command.TotalCol,
                DefaultSeatTypeId: null);

            var createSeatsResult = await _sender.Send(createSeatsCommand, cancellationToken);

            if (!createSeatsResult.IsSuccess)
            {
                // Rollback: Mark room as inactive if seat creation fails
                room.IsActive = false;
                _roomRepo.Update(room);
                await _uow.SaveChangesAsync(cancellationToken);

                return Result.Failure<Guid>(
                    new Error("Seats.CreationFailed",
                        $"Failed to create seats for room '{room.Id}'."));
            }

            return Result.Success(room.Id);
        }
    }
}
