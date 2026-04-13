using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatMap
{
    public sealed class GetSeatMapHandler
        : IQueryHandler<GetSeatMap.Query, SeatMapDto>
    {
        private readonly ShowingSeatValidator _validator;
        private readonly IShowingSeatRepository _showingSeatRepo;

        public GetSeatMapHandler(
            IShowingRepository showingRepo,
            IRoomRepository roomRepo,
            IShowingSeatRepository showingSeatRepo)
        {
            _validator = new ShowingSeatValidator(showingRepo, roomRepo);
            _showingSeatRepo = showingSeatRepo;
        }

        public async Task<Result<SeatMapDto>> Handle(
            GetSeatMap.Query query,
            CancellationToken cancellationToken)
        {
            // Validate showing + room + match + get entities
            var validationResult = await _validator.ValidateAndGetAsync(
                query.ShowingId, query.RoomId, cancellationToken);

            if (!validationResult.IsSuccess)
                return Result.Failure<SeatMapDto>(validationResult.Error);

            var (showing, room) = validationResult.Value;

            // Get all showing seats for this showing
            var showingSeats = await _showingSeatRepo.GetSeatMapAsync(
                query.ShowingId, cancellationToken);

            // Group by row with full detail
            var seatsByRow = showingSeats
                .GroupBy(ss => ss.Seat!.RowIndex)
                .OrderBy(g => g.Key)
                .Select(rowGroup => new SeatRowDto(
                    rowGroup.Key,
                    rowGroup.First().Seat!.RowName,
                    rowGroup
                        .OrderBy(ss => ss.Seat!.ColIndex)
                        .Select(ss => new SeatStatusDto(
                            ss.Id,
                            ss.SeatId,
                            ss.Seat!.RowIndex,
                            ss.Seat!.ColIndex,
                            ss.Seat!.RowName,
                            ss.Seat!.SeatNumber,
                            ss.Seat!.SeatTypeId,
                            ss.Seat!.SeatType?.Name ?? "Unknown",
                            ss.Seat!.SeatType?.Surcharge ?? 0,
                            ss.Price,
                            ss.Status.ToString(),
                            ss.LockedUntil))
                        .ToList()))
                .ToList();

            // Calculate statistics
            var availableCount = showingSeats.Count(ss => ss.Status.ToString() == "Available");
            var lockedCount = showingSeats.Count(ss => ss.Status.ToString() == "Locked");
            var bookedCount = showingSeats.Count(ss => ss.Status.ToString() == "Booked");

            var seatMapDto = new SeatMapDto(
                query.ShowingId,
                query.RoomId,
                room.Name,
                room.TotalRow,
                room.TotalCol,
                room.TotalSeat,
                showing.Price,
                availableCount,
                lockedCount,
                bookedCount,
                seatsByRow);

            return Result.Success(seatMapDto);
        }
    }
}
