using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Theater.UpdateTheater
{
    public sealed class UpdateTheaterHandler
        : ICommandHander<UpdateTheater.Command>
    {
        private readonly ITheaterRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateTheaterHandler(ITheaterRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<Result> Handle(
            UpdateTheater.Command command,
            CancellationToken cancellationToken)
        {
            var theater = await _repo.GetByIdAsync(command.Id, cancellationToken);

            if (theater is null)
                return Result.Failure(
                    new Error("Theater.NotFound", $"Theater '{command.Id}' not found."));

            // Check if name is already used by another theater (case-insensitive, excluding current theater)
            var nameExists = await _repo.AnyAsync(
                t => t.Name.ToLower() == command.Name.ToLower() && t.Id != command.Id,
                cancellationToken);

            if (nameExists)
                return Result.Failure(
                    new Error("Theater.DuplicateName",
                        $"Theater name '{command.Name}' is already in use."));

            theater.Name = command.Name;
            theater.Location = command.Location;
            theater.IsActive = command.IsActive;
            theater.DateUpdate = DateTime.UtcNow;

            _repo.Update(theater);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
