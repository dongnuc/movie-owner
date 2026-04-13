using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Room.UpdateRoom
{
    public sealed class UpdateRoomHandler
        : ICommandHander<UpdateRoom.Command>
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IUnitOfWork _uow;

        public UpdateRoomHandler(IRoomRepository roomRepo, IUnitOfWork uow)
        {
            _roomRepo = roomRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(
            UpdateRoom.Command command,
            CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetByIdAsync(command.Id, cancellationToken);

            if (room is null)
                return Result.Failure(
                    new Error("Room.NotFound", $"Room '{command.Id}' not found."));

            // Check if new name already exists in the same theater (case-insensitive, excluding current room)
            var duplicateName = await _roomRepo.AnyAsync(
                r => r.TheaterId == room.TheaterId && 
                     r.Name.ToLower() == command.Name.ToLower() && 
                     r.Id != command.Id,
                cancellationToken);

            if (duplicateName)
                return Result.Failure(
                    new Error("Room.DuplicateName",
                        $"Room name '{command.Name}' already exists in this theater."));

            room.Name = command.Name;
            room.IsActive = command.IsActive;
            room.DateUpdate = DateTime.UtcNow;

            _roomRepo.Update(room);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
