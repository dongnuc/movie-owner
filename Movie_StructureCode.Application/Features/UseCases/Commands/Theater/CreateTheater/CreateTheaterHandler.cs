using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Theater.CreateTheater
{
    public sealed class CreateTheaterHandler
        : ICommandHander<CreateTheater.Command, Guid>
    {
        private readonly ITheaterRepository _repo;
        private readonly IUnitOfWork        _uow;

        public CreateTheaterHandler(ITheaterRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        public async Task<Result<Guid>> Handle(
            CreateTheater.Command command,
            CancellationToken     cancellationToken)
        {
            var exists = await _repo.AnyAsync(
                t => t.Name == command.Name, cancellationToken);

            if (exists)
                return Result.Failure<Guid>(
                    new Error("Theater.Duplicate",
                        $"Theater '{command.Name}' already exists."));

            var theater = new Domain.Entities.Theater
            {
                Id         = Guid.NewGuid(),
                Name       = command.Name,
                Location   = command.Location,
                IsActive   = true,
                DateCreate = DateTime.UtcNow,
                DateUpdate = DateTime.UtcNow
            };

            await _repo.AddAsync(theater, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(theater.Id);
        }
    }
}
