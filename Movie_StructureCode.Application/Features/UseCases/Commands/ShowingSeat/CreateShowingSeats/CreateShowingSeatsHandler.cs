using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.CreateShowingSeats
{
    public sealed class CreateShowingSeatsHandler
        : ICommandHander<CreateShowingSeats.Command, CreateShowingSeatsResult>
    {
        private readonly ShowingSeatValidator _validator;
        private readonly ISeatRepository _seatRepo;
        private readonly IShowingSeatRepository _showingSeatRepo;
        private readonly IUnitOfWork _uow;

        public CreateShowingSeatsHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            ISeatRepository seatRepo,
            IShowingSeatRepository showingSeatRepo,
            IUnitOfWork uow)
        {
            _validator = new ShowingSeatValidator(showingRepo, roomRepo);
            _seatRepo = seatRepo;
            _showingSeatRepo = showingSeatRepo;
            _uow = uow;
        }

        public async Task<Result<CreateShowingSeatsResult>> Handle(
            CreateShowingSeats.Command command,
            CancellationToken cancellationToken)
        {
            // Validate showing + room + match
            var validationResult = await _validator.ValidateShowingAndRoomAsync(
                command.ShowingId, command.RoomId, cancellationToken);

            if (!validationResult.IsSuccess)
                return Result.Failure<CreateShowingSeatsResult>(validationResult.Error);

            // Get all seats in the room
            var seats = await _seatRepo.FindAsync(s => s.RoomId == command.RoomId, cancellationToken);
            var seatsList = seats.ToList();

            if (!seatsList.Any())
                return Result.Failure<CreateShowingSeatsResult>(
                    new Error("Seats.NotFound", $"No seats found in room '{command.RoomId}'."));

            // Parse locked row indices
            var lockedRowIndices = command.LockedRowIndices?.ToHashSet() ?? new HashSet<int>();

            // Create showing seats
            var now = DateTime.UtcNow;
            var lockDuration = TimeSpan.FromMinutes(10);
            var createdShowingSeats = new List<Domain.Entities.ShowingSeat>();
            var createdIds = new List<Guid>();
            int lockedCount = 0;

            foreach (var seat in seatsList)
            {
                var isRowLocked = lockedRowIndices.Contains(seat.RowIndex);
                var showingSeatId = Guid.NewGuid();

                var showingSeat = new Domain.Entities.ShowingSeat
                {
                    Id = showingSeatId,
                    ShowingId = command.ShowingId,
                    SeatId = seat.Id,
                    Price = command.Price,
                    Status = isRowLocked ? ShowingSeatStatus.Locked : ShowingSeatStatus.Available,
                    LockedUntil = isRowLocked ? now.Add(lockDuration) : null,
                    DateCreate = now,
                    DateUpdate = now,
                    IsActive = true
                };

                createdShowingSeats.Add(showingSeat);
                createdIds.Add(showingSeatId);

                if (isRowLocked)
                    lockedCount++;
            }

            // Add all showing seats to repository
            foreach (var showingSeat in createdShowingSeats)
            {
                await _showingSeatRepo.AddAsync(showingSeat, cancellationToken);
            }

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateShowingSeatsResult(
                createdShowingSeats.Count,
                lockedCount,
                createdIds));
        }
    }
}
