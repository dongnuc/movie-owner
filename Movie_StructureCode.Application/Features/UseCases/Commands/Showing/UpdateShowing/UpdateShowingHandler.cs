using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Showing.UpdateShowing
{
    public sealed class UpdateShowingHandler
        : ICommandHander<UpdateShowing.Command>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IUnitOfWork _uow;

        public UpdateShowingHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            IUnitOfWork uow)
        {
            _showingRepo = showingRepo;
            _roomRepo = roomRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(
            UpdateShowing.Command command,
            CancellationToken cancellationToken)
        {
            var showing = await _showingRepo.GetByIdAsync(command.Id, cancellationToken);

            if (showing is null)
                return Result.Failure(
                    new Error("Showing.NotFound", $"Showing '{command.Id}' not found."));

            // If changing time, check for conflicts
            if (showing.TimeStart != command.TimeStart)
            {
                var roomShowings = await _showingRepo.GetByRoomAsync(showing.RoomId, cancellationToken);
                var movie = showing.Movie;
                var timeEnd = command.TimeStart.AddMinutes(movie?.Duration ?? 0);

                var hasConflict = roomShowings
                    .Where(s => s.Id != command.Id)
                    .Any(s =>
                        s.TimeStart < timeEnd &&
                        s.TimeStart.AddMinutes(s.Movie?.Duration ?? 0) > command.TimeStart);

                if (hasConflict)
                    return Result.Failure(
                        new Error("Showing.TimeConflict",
                            "Another showing exists at this time in this room."));
            }

            showing.TimeStart = command.TimeStart;
            showing.Price = command.Price;
            showing.MaxTicketsPerUser = command.MaxTicketsPerUser;
            showing.IsActive = command.IsActive;
            showing.DateUpdate = DateTime.UtcNow;

            _showingRepo.Update(showing);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
