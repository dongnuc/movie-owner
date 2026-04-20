using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using MediatR;
using Movie_StructureCode.Application.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.CreateSeatType
{
    public sealed class CreateSeatTypeHandler
        : ICommandHander<CreateSeatType.Command, Guid>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public CreateSeatTypeHandler(ISeatTypeRepository seatTypeRepo, IUnitOfWork uow, IMediator mediator)
        {
            _seatTypeRepo = seatTypeRepo;
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Result<Guid>> Handle(
            CreateSeatType.Command command,
            CancellationToken cancellationToken)
        {
            try
            {
                // Check if seat type with same name already exists (case-insensitive)
                var existingName = await _seatTypeRepo.AnyAsync(
                    st => st.Name.ToLower() == command.Name.ToLower(),
                    cancellationToken);

                if (existingName)
                {
                    return Result.Failure<Guid>(
                        new Error("SeatType.DuplicateName",
                            $"SeatType with name '{command.Name}' already exists."));
                }

                // Create new seat type
                var seatType = new Domain.Entities.SeatType
                {
                    Id = Guid.NewGuid(),
                    Name = command.Name,
                    Surcharge = command.Surcharge,
                    IsActive = true,
                    DateCreate = DateTime.UtcNow,
                    DateUpdate = DateTime.UtcNow
                };

                await _seatTypeRepo.AddAsync(seatType, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                // Publish cache invalidation event for seat type list (global)
                await _mediator.Publish(new EntityChangedEvent("seattype",null), cancellationToken);

                return Result.Success(seatType.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(
                    new Error("SeatType.Error", $"An error occurred while creating seat type: {ex.Message}"));
            }
        }
    }
}
