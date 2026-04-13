using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.UnlockSeats
{
    public sealed class UnlockSeatsHandler
        : ICommandHander<UnlockSeats.Command, UnlockSeatsResult>
    {
        private readonly ShowingSeatValidator _validator;
        private readonly IShowingSeatRepository _showingSeatRepo;
        private readonly IUnitOfWork _uow;

        public UnlockSeatsHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            IShowingSeatRepository showingSeatRepo,
            IUnitOfWork uow)
        {
            _validator = new ShowingSeatValidator(showingRepo, roomRepo);
            _showingSeatRepo = showingSeatRepo;
            _uow = uow;
        }

        public async Task<Result<UnlockSeatsResult>> Handle(
            UnlockSeats.Command command,
            CancellationToken cancellationToken)
        {
            // Validate showing + room + match
            var validationResult = await _validator.ValidateShowingAndRoomAsync(
                command.ShowingId, command.RoomId, cancellationToken);

            if (!validationResult.IsSuccess)
                return Result.Failure<UnlockSeatsResult>(validationResult.Error);

            // Get all showing seats for this showing
            var showingSeats = await _showingSeatRepo.GetByShowingAsync(
                command.ShowingId, cancellationToken);

            var seatIdsToUnlock = command.SeatIds.ToHashSet();
            var seatsToUnlock = new List<Domain.Entities.ShowingSeat>();
            var failedSeatNumbers = new List<string>();

            foreach (var seatId in seatIdsToUnlock)
            {
                var showingSeat = showingSeats.FirstOrDefault(ss => ss.SeatId == seatId);

                if (showingSeat is null)
                {
                    failedSeatNumbers.Add($"Seat {seatId} not found");
                    continue;
                }

                // Ch? unlock gh? ?ang b? lock
                if (showingSeat.Status.ToString() == "Locked")
                {
                    seatsToUnlock.Add(showingSeat);
                }
                else
                {
                    failedSeatNumbers.Add(
                        $"{showingSeat.Seat?.RowName}{showingSeat.Seat?.SeatNumber} - " +
                        $"Status: {showingSeat.Status} (ch? unlock gh? Locked)");
                }
            }

            // Unlock seats
            var now = DateTime.UtcNow;

            foreach (var seat in seatsToUnlock)
            {
                seat.Status = Domain.Entities.ShowingSeatStatus.Available;
                seat.LockedUntil = null;
                seat.DateUpdate = now;
            }

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(new UnlockSeatsResult(
                seatsToUnlock.Count,
                failedSeatNumbers.Count,
                failedSeatNumbers));
        }
    }
}
