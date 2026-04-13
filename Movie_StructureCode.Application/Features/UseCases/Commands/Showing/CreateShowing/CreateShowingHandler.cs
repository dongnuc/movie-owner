using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using MediatR;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.CreateShowingSeats;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Showing.CreateShowing
{
    public sealed class CreateShowingHandler
        : ICommandHander<CreateShowing.Command, Guid>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IUnitOfWork _uow;
        private readonly ISender _sender;

        public CreateShowingHandler(
            IShowingRepository showingRepo,
            IMovieRepository movieRepo,
            IRoomRepository roomRepo,
            IUnitOfWork uow,
            ISender sender)
        {
            _showingRepo = showingRepo;
            _movieRepo = movieRepo;
            _roomRepo = roomRepo;
            _uow = uow;
            _sender = sender;
        }

        public async Task<Result<Guid>> Handle(
            CreateShowing.Command command,
            CancellationToken cancellationToken)
        {
            // Verify movie exists and is active
            var movie = await _movieRepo.GetByIdAsync(command.MovieId, cancellationToken);
            if (movie is null || !movie.IsActive)
                return Result.Failure<Guid>(
                    new Error("Movie.NotFoundOrInactive",
                        $"Movie '{command.MovieId}' not found or is inactive."));

            // Verify room exists
            var room = await _roomRepo.GetByIdAsync(command.RoomId, cancellationToken);
            if (room is null)
                return Result.Failure<Guid>(
                    new Error("Room.NotFound", $"Room '{command.RoomId}' not found."));

            // Verify no overlapping showings in the room
            var roomShowings = await _showingRepo.GetByRoomAsync(command.RoomId, cancellationToken);
            var timeEnd = command.TimeStart.AddMinutes(movie.Duration);

            var hasConflict = roomShowings.Any(s =>
                s.TimeStart < timeEnd && s.TimeStart.AddMinutes(s.Movie?.Duration ?? 0) > command.TimeStart);

            if (hasConflict)
                return Result.Failure<Guid>(
                    new Error("Showing.TimeConflict",
                        "Another showing exists at this time in this room."));

            // Create showing
            var newShowing = new Domain.Entities.Showing
            {
                Id = Guid.NewGuid(),
                MovieId = command.MovieId,
                RoomId = command.RoomId,
                TimeStart = command.TimeStart,
                Price = command.Price,
                MaxTicketsPerUser = command.MaxTicketsPerUser,
                IsActive = true,
                DateCreate = DateTime.UtcNow,
                DateUpdate = DateTime.UtcNow
            };

            await _showingRepo.AddAsync(newShowing, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            // Create ShowingSeats using CreateShowingSeats usecase
            var createShowingSeatsCommand = new CreateShowingSeats.Command(
                newShowing.Id,
                command.RoomId,
                command.Price,
                LockedRowIndices: null);  

            var createShowingSeatsResult = await _sender.Send(createShowingSeatsCommand, cancellationToken);

            if (!createShowingSeatsResult.IsSuccess)
            {
                newShowing.IsActive = false;
                _showingRepo.Update(newShowing);
                await _uow.SaveChangesAsync(cancellationToken);

                return Result.Failure<Guid>(
                    new Error("ShowingSeats.CreationFailed",
                        $"Failed to create ShowingSeats for Showing '{newShowing.Id}'."));
            }

            return Result.Success(newShowing.Id);
        }
    }
}
