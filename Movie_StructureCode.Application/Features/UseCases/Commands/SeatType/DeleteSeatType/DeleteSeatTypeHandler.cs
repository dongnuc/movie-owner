using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using MediatR;
using Movie_StructureCode.Application.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.DeleteSeatType
{
    public sealed class DeleteSeatTypeHandler
        : ICommandHander<DeleteSeatType.Command>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public DeleteSeatTypeHandler(ISeatTypeRepository seatTypeRepo, IUnitOfWork uow, IMediator mediator)
        {
            _seatTypeRepo = seatTypeRepo;
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Result> Handle(
            DeleteSeatType.Command command,
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

                // Check if seat type is being used by any seats
                var isInUse = await _seatTypeRepo.AnyAsync(
                    st => st.Id == command.Id && st.Seats != null && st.Seats.Count > 0,
                    cancellationToken);

                if (isInUse)
                {
                    return Result.Failure(
                        new Error("SeatType.InUse",
                            "Cannot delete SeatType because it is being used by seats."));
                }

                _seatTypeRepo.Remove(seatType);
                await _uow.SaveChangesAsync(cancellationToken);

                // Publish cache invalidation event
                // Invalidate both global seat type list and specific seat type cache
                await _mediator.Publish(new EntityChangedEvent("seattype", command.Id), cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(
                    new Error("SeatType.Error", $"An error occurred while deleting seat type: {ex.Message}"));
            }
        }
    }
}
