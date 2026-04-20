using MediatR;
using Movie_StructureCode.Application.Common;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.UpdateSeatType
{
    public sealed class UpdateSeatTypeHandler
        : ICommandHander<UpdateSeatType.Command>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public UpdateSeatTypeHandler(ISeatTypeRepository seatTypeRepo, IUnitOfWork uow, IMediator mediator)
        {
            _seatTypeRepo = seatTypeRepo;
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Result> Handle(
            UpdateSeatType.Command command,
            CancellationToken cancellationToken)
        {
            try
            {
                var seatType = await _seatTypeRepo.GetByIdAsync(command.Id, cancellationToken);

                if (seatType is null)
                {
                    return Result.Failure(
                        new Error("SeatType.NotFound", $"SeatType with ID '{command.Id}' not found."));
                }

                // Check if new name already exists (excluding current seat type)
                var duplicateName = await _seatTypeRepo.AnyAsync(
                    st => st.Name.ToLower() == command.Name.ToLower() && st.Id != command.Id,
                    cancellationToken);

                if (duplicateName)
                {
                    return Result.Failure(
                        new Error("SeatType.DuplicateName",
                            $"SeatType with name '{command.Name}' already exists."));
                }

                seatType.Name = command.Name;
                seatType.Surcharge = command.Surcharge;
                seatType.IsActive = command.IsActive;
                seatType.DateUpdate = DateTime.UtcNow;

                _seatTypeRepo.Update(seatType);
                await _uow.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntityChangedEvent("seattype",seatType.Id));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(
                    new Error("SeatType.Error", $"An error occurred while updating seat type: {ex.Message}"));
            }
        }
    }
}
