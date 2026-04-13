using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.MarkBrokenSeats
{
    public sealed class MarkBrokenSeatsHandler
        : ICommandHander<MarkBrokenSeats.Command, MarkBrokenSeatsResult>
    {
        private readonly ShowingSeatValidator _validator;
        private readonly IShowingSeatRepository _showingSeatRepo;
        private readonly IUnitOfWork _uow;

        public MarkBrokenSeatsHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            IShowingSeatRepository showingSeatRepo,
            IUnitOfWork uow)
        {
            _validator = new ShowingSeatValidator(showingRepo, roomRepo);
            _showingSeatRepo = showingSeatRepo;
            _uow = uow;
        }

        public async Task<Result<MarkBrokenSeatsResult>> Handle(
            MarkBrokenSeats.Command command,
            CancellationToken cancellationToken)
        {
            // Validate showing + room + match
            var validationResult = await _validator.ValidateShowingAndRoomAsync(
                command.ShowingId, command.RoomId, cancellationToken);

            if (!validationResult.IsSuccess)
                return Result.Failure<MarkBrokenSeatsResult>(validationResult.Error);

            // Get all showing seats for this showing
            var showingSeats = await _showingSeatRepo.GetByShowingAsync(
                command.ShowingId, cancellationToken);

            var seatIdsToMark = command.SeatIds.ToHashSet();
            var seatsToMark = new List<Domain.Entities.ShowingSeat>();
            var failedSeatNumbers = new List<string>();

            foreach (var seatId in seatIdsToMark)
            {
                var showingSeat = showingSeats.FirstOrDefault(ss => ss.SeatId == seatId);

                if (showingSeat is null)
                {
                    failedSeatNumbers.Add($"Seat {seatId} not found");
                    continue;
                }

                // Không th? mark gh? ?ã booked
                if (showingSeat.Status.ToString() == "Booked")
                {
                    failedSeatNumbers.Add(
                        $"{showingSeat.Seat?.RowName}{showingSeat.Seat?.SeatNumber} - " +
                        $"?ã ???c ??t, không th? ?ánh d?u h?ng");
                    continue;
                }

                seatsToMark.Add(showingSeat);
            }

            // Mark seats as broken (lock vô th?i h?n)
            var now = DateTime.UtcNow;
            var farFutureDate = now.AddYears(100);  // N?m 2124 - effectively permanent

            foreach (var seat in seatsToMark)
            {
                seat.Status = Domain.Entities.ShowingSeatStatus.Locked;
                seat.LockedUntil = farFutureDate;  // Lock vô th?i h?n
                seat.DateUpdate = now;
                seat.IsActive = false;  // Mark as inactive
            }

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(new MarkBrokenSeatsResult(
                seatsToMark.Count,
                failedSeatNumbers.Count,
                failedSeatNumbers));
        }
    }
}
