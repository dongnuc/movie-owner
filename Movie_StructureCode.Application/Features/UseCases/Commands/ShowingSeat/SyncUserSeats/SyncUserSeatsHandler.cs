using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;


namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.SyncUserSeats
{
    public sealed class SyncUserSeatsHandler :
        ICommandHander<SyncUserSeatsRequest.Command, SyncUserSeatsResult>
    {
        private readonly ISeatLockService _lockService;
        private readonly IShowingRepository _showingRepo;

        public SyncUserSeatsHandler(ISeatLockService lockService, IShowingRepository showingRepo)
        {
            _lockService = lockService;
            _showingRepo = showingRepo;
        }

        public async Task<Result<SyncUserSeatsResult>> Handle(SyncUserSeatsRequest.Command request,
            CancellationToken cancellationToken)
        {
            var showingId = request.ShowingId;
            var seatIds = request.SelectedSeatIds?.ToList() ?? new List<Guid>();
            var userId = request.UserId;

            var isShowingIdValid =
                await _showingRepo.AnyAsync(s => s.Id == showingId, cancellationToken);
            if(!isShowingIdValid)
            {
                return Result.Failure<SyncUserSeatsResult>(new Error("Showing.Invalid", $"Showing with ID '{showingId}' not found."));
            }


            var currentsSeats = (await _lockService.GetUserLockedSeatsAsync(showingId, userId))
                .Select(Guid.Parse)
                .ToList();

            //diff
            var toAdd = seatIds.Except(currentsSeats).ToList();
            var toRemove = currentsSeats.Except(seatIds).ToList();

            var lockedSeats = new List<string>();
            var failedSeats = new List<string>();

            if (toAdd.Any())
            {
                var lockResult = await _lockService.LockSeatsAsync(showingId, toAdd.Select(s => s.ToString()).ToList(), userId);
                if (!lockResult)
                {
                    return Result.Success(new SyncUserSeatsResult(
                        false,
                        $"Failed to lock seats: {string.Join(", ", toAdd)}"
                    ));
                }
            }

            if (toRemove.Any())
            {
                var unlockResult = await _lockService.UnlockSeatsAsync(showingId, toRemove.Select(s => s.ToString()).ToList(), userId);
                if (!unlockResult)
                {
                    failedSeats.Add("Failed to unlock seats: " + string.Join(", ", toRemove));
                }
            }

            var finalSeats = await _lockService.GetUserLockedSeatsAsync(showingId, userId);

            return Result.Success(new SyncUserSeatsResult
            (
                 true,
                 finalSeats.Count > 0
                    ? $"Locked seats: {string.Join(", ", finalSeats)}"
                    : "No seats locked"
            ));

        }
    }
}
