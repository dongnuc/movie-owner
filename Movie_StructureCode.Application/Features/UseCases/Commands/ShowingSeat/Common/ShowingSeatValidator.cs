using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common
{
    /// <summary>
    /// Validator chung cho các use case liên quan ShowingSeat
    /// T?p trung x? lý validation: Showing, Room, Room Match
    /// </summary>
    public sealed class ShowingSeatValidator
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IRoomRepository _roomRepo;

        public ShowingSeatValidator(IShowingRepository showingRepo, IRoomRepository roomRepo)
        {
            _showingRepo = showingRepo;
            _roomRepo = roomRepo;
        }

        /// <summary>
        /// Validate Showing + Room + Match
        /// </summary>
        public async Task<Result> ValidateShowingAndRoomAsync(
            Guid showingId,
            Guid roomId,
            CancellationToken cancellationToken)
        {
            // Verify showing exists
            var showing = await _showingRepo.GetByIdAsync(showingId, cancellationToken);
            if (showing is null)
                return Result.Failure(
                    new Error("Showing.NotFound", $"Showing '{showingId}' not found."));

            // Verify room exists
            var room = await _roomRepo.GetByIdAsync(roomId, cancellationToken);
            if (room is null)
                return Result.Failure(
                    new Error("Room.NotFound", $"Room '{roomId}' not found."));

            // Verify room matches showing
            if (showing.RoomId != roomId)
                return Result.Failure(
                    new Error("Room.Mismatch",
                        $"Room '{roomId}' does not match Showing's room."));

            return Result.Success();
        }

        /// <summary>
        /// Validate Showing + Room + Match + Return entities
        /// </summary>
        public async Task<Result<(Domain.Entities.Showing Showing, Domain.Entities.Room Room)>> ValidateAndGetAsync(
            Guid showingId,
            Guid roomId,
            CancellationToken cancellationToken)
        {
            // Verify showing exists
            var showing = await _showingRepo.GetByIdAsync(showingId, cancellationToken);
            if (showing is null)
                return Result.Failure<(Domain.Entities.Showing, Domain.Entities.Room)>(
                    new Error("Showing.NotFound", $"Showing '{showingId}' not found."));

            // Verify room exists
            var room = await _roomRepo.GetByIdAsync(roomId, cancellationToken);
            if (room is null)
                return Result.Failure<(Domain.Entities.Showing, Domain.Entities.Room)>(
                    new Error("Room.NotFound", $"Room '{roomId}' not found."));

            // Verify room matches showing
            if (showing.RoomId != roomId)
                return Result.Failure<(Domain.Entities.Showing, Domain.Entities.Room)>(
                    new Error("Room.Mismatch",
                        $"Room '{roomId}' does not match Showing's room."));

            return Result.Success((showing, room));
        }
    }
}
