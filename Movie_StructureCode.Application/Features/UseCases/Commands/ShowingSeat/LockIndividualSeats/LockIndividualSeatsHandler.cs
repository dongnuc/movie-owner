using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.LockIndividualSeats
{
    public sealed class LockIndividualSeatsHandler
        : ICommandHander<LockIndividualSeats.Command, LockIndividualSeatsResult>
    {
        private readonly ShowingSeatValidator _validator;
        private readonly IShowingSeatRepository _showingSeatRepo;
        private readonly IUnitOfWork _uow;
        private static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(10);

        public LockIndividualSeatsHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            IShowingSeatRepository showingSeatRepo,
            IUnitOfWork uow)
        {
            _validator = new ShowingSeatValidator(showingRepo, roomRepo);
            _showingSeatRepo = showingSeatRepo;
            _uow = uow;
        }

        public async Task<Result<LockIndividualSeatsResult>> Handle(
            LockIndividualSeats.Command command,
            CancellationToken cancellationToken)
        {
            // Validate showing + room + match
            var validationResult = await _validator.ValidateShowingAndRoomAsync(
                command.ShowingId, command.RoomId, cancellationToken);

            if (!validationResult.IsSuccess)
                return Result.Failure<LockIndividualSeatsResult>(validationResult.Error);

            // Get all showing seats for this showing
            var showingSeats = await _showingSeatRepo.GetByShowingAsync(
                command.ShowingId, cancellationToken);

            var seatIdsToLock = command.SeatIds.ToHashSet();
            var seatsToLock = new List<Domain.Entities.ShowingSeat>();
            var failedSeatNumbers = new List<string>();

            foreach (var seatId in seatIdsToLock)
            {
                var showingSeat = showingSeats.FirstOrDefault(ss => ss.SeatId == seatId);
                
                if (showingSeat is null)
                {
                    failedSeatNumbers.Add($"Seat {seatId} not found");
                    continue;
                }

                // Ch? lock gh? Available ho?c lock h?t h?n
                if (showingSeat.Status.ToString() == "Available" || 
                    (showingSeat.Status.ToString() == "Locked" && showingSeat.LockedUntil <= DateTime.UtcNow))
                {
                    seatsToLock.Add(showingSeat);
                }
                else
                {
                    failedSeatNumbers.Add(
                        $"{showingSeat.Seat?.RowName}{showingSeat.Seat?.SeatNumber} - " +
                        $"Status: {showingSeat.Status}");
                }
            }

            // Lock seats
            var now = DateTime.UtcNow;
            var lockedUntil = now.Add(LockDuration);

            foreach (var seat in seatsToLock)
            {
                seat.Status = Domain.Entities.ShowingSeatStatus.Locked;
                seat.LockedUntil = lockedUntil;
                seat.DateUpdate = now;
            }

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(new LockIndividualSeatsResult(
                seatsToLock.Count,
                failedSeatNumbers.Count,
                lockedUntil,
                failedSeatNumbers));
        }
    }
}
