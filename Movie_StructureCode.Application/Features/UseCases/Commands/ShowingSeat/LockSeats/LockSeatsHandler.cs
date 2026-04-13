using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.LockSeats
{
    public sealed class LockSeatsHandler
        : ICommandHander<LockSeats.Command, LockSeatsResult>
    {
        private readonly ShowingSeatValidator _validator;
        private readonly IShowingSeatRepository _showingSeatRepo;
        private readonly IUnitOfWork _uow;
        private static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(10);

        public LockSeatsHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            IShowingSeatRepository showingSeatRepo,
            IUnitOfWork uow)
        {
            _validator = new ShowingSeatValidator(showingRepo, roomRepo);
            _showingSeatRepo = showingSeatRepo;
            _uow = uow;
        }

        public async Task<Result<LockSeatsResult>> Handle(
            LockSeats.Command command,
            CancellationToken cancellationToken)
        {
            // Validate showing + room + match
            var validationResult = await _validator.ValidateShowingAndRoomAsync(
                command.ShowingId, command.RoomId, cancellationToken);

            if (!validationResult.IsSuccess)
                return Result.Failure<LockSeatsResult>(validationResult.Error);

            // Get all showing seats for this showing
            var showingSeats = await _showingSeatRepo.GetByShowingAsync(
                command.ShowingId, cancellationToken);

            var rowIndicesToLock = command.RowIndices.ToHashSet();
            var seatsToLock = showingSeats
                .Where(ss => rowIndicesToLock.Contains(ss.Seat!.RowIndex))
                .ToList();

            if (!seatsToLock.Any())
                return Result.Failure<LockSeatsResult>(
                    new Error("Seats.NotFound",
                        $"No seats found for the specified rows in showing '{command.ShowingId}'."));

            // Lock seats
            var now = DateTime.UtcNow;
            var lockedUntil = now.Add(LockDuration);

            foreach (var seat in seatsToLock)
            {
                if (seat.Status.ToString() == "Available" || 
                    (seat.Status.ToString() == "Locked" && seat.LockedUntil <= now))
                {
                    seat.Status = Domain.Entities.ShowingSeatStatus.Locked;
                    seat.LockedUntil = lockedUntil;
                    seat.DateUpdate = now;
                }
            }

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(new LockSeatsResult(seatsToLock.Count, lockedUntil));
        }
    }
}
