using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using System.Threading.Channels;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Seat.UpdateSeats
{
    /// <summary>
    /// Handler ?? batch update seats
    /// </summary>
    public sealed class UpdateSeatsHandler
        : ICommandHander<UpdateSeats.Command, BatchUpdateSeatsResultDto>
    {
        private readonly ISeatRepository _seatRepo;
        private readonly ISeatTypeRepository _seatTypeRepo;
        private readonly IUnitOfWork _uow;
        private readonly ISeatValidator _seatValidator;
        private readonly ISeatLayoutService _seatLayoutService;
        public UpdateSeatsHandler(
            ISeatRepository seatRepo,
            ISeatTypeRepository seatTypeRepo,
            IUnitOfWork uow,
            ISeatValidator seatValidator,
            ISeatLayoutService seatLayoutService)
        {
            _seatRepo = seatRepo;
            _seatTypeRepo = seatTypeRepo;
            _uow = uow;
            _seatValidator = seatValidator;
            _seatLayoutService = seatLayoutService;
        }

        private bool UpdateSeatEntity(
            Movie_StructureCode.Domain.Entities.Seat seat,
            UpdateSeatDto seatDto,
            Guid coupleSeatTypeId)
        {
            var isOldCouple = seat.SeatTypeId == coupleSeatTypeId;
            var isNewCouple = seatDto.SeatTypeId == coupleSeatTypeId;

            var isLayoutChanged = isOldCouple != isNewCouple;

            if (!isNewCouple)
            {
                // Couple → Single
                seat.GroupId = null;
            }
            else
            {
                // Single → Couple hoặc giữ nguyên
                seat.GroupId = seatDto.GroupId ?? Guid.NewGuid();
            }

            seat.SeatTypeId = seatDto.SeatTypeId;
            seat.StatusSeat = (StatusSeat)seatDto.StatusSeat;
            seat.DateUpdate = DateTime.UtcNow;


            return isLayoutChanged;
        }

        public async Task<Result<BatchUpdateSeatsResultDto>> Handle(
            UpdateSeats.Command command,
            CancellationToken cancellationToken)
        {
            await using var transaction = await _uow.BeginTransactionAsync(cancellationToken);

            try
            {
                bool needRecalculate = false;
                var successCount = 0;
                var failureCount = 0;
                var failureReasons = new List<string>();

                // STEP 1: Validate input (not empty)
                var seatsToUpdate = command.Seats.ToList();

                if (!seatsToUpdate.Any())
                {
                    return Result.Failure<BatchUpdateSeatsResultDto>(
                        new Error("Seat.EmptyList", "No seats provided for update."));
                }

                // STEP 2: Extract unique Seat IDs
                var seatIds = seatsToUpdate
                    .Select(s => s.Id)
                    .Distinct()
                    .ToList();

                // STEP 3: Load ONLY seats c?n update t? DB (LINQ filter)
                var existingSeats = (await _seatRepo.FindAsync(
                    s => seatIds.Contains(s.Id),
                    cancellationToken)).ToList();

                // Map existing seats by ID for O(1) lookup
                var seatMap = existingSeats.ToDictionary(s => s.Id);

                // STEP 4: Extract unique SeatType IDs
                var seatTypeIds = seatsToUpdate
                    .Select(s => s.SeatTypeId)
                    .Distinct()
                    .ToList();

                // STEP 5: Load ONLY SeatTypes c?n t? DB (LINQ filter)
                var existingSeatTypes = (await _seatTypeRepo.FindAsync(
                    st => seatTypeIds.Contains(st.Id),
                    cancellationToken)).ToList();

                var coupleSeatType = (await _seatTypeRepo.FindAsync(
                    st => st.Name.ToLower() == "couple",
                    cancellationToken)).FirstOrDefault()
                    ?? throw new Exception("Couple seat type not found");

                var coupleSeatTypeId = coupleSeatType.Id;

                var seatTypeMap = existingSeatTypes.ToDictionary(st => st.Id);

                // STEP 6: Validate & Update trên memory (không save)
                // ===== PROCESS =====
                foreach (var seatDto in seatsToUpdate)
                {
                    if (!_seatValidator.TryValidate(seatDto, seatMap, seatTypeMap, out var error))
                    {
                        failureCount++;
                        failureReasons.Add(error!);
                        continue;
                    }

                    var seat = seatMap[seatDto.Id];
                    var changed = UpdateSeatEntity(seat, seatDto, coupleSeatTypeId);
                    if (changed)
                        needRecalculate = true;
                     _seatRepo.Update(seat);

                    successCount++;

                    if (failureCount > 0)
                    {
                        await transaction.RollbackAsync(cancellationToken);

                        return Result.Failure<BatchUpdateSeatsResultDto>(
                            new Error("Seat.ValidationFailed",
                                $"Failed {failureCount} seats. Details: {string.Join("; ", failureReasons)}"));
                    }

                    if (needRecalculate)
                    {
                        var rows = existingSeats
                            .GroupBy(s => new { s.RowIndex, s.RowName })
                            .Select(g => g.ToList());

                        foreach (var row in rows)
                        {
                            _seatLayoutService.RecalculateSeatNumbers(row, coupleSeatTypeId);
                        }
                    }

                }

                await _uow.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success(new BatchUpdateSeatsResultDto(successCount, 0, new List<string>()));

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return Result.Failure<BatchUpdateSeatsResultDto>(
                    new Error("Seat.Error", ex.Message));
            }
        }
    }
}
