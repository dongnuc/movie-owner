using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatCount
{
    public sealed class GetSeatCountHandler
        : IQueryHandler<GetSeatCount.Query, GetSeatCount.SeatCountDto>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IShowingSeatRepository _showingSeatRepo;

        public GetSeatCountHandler(
            IShowingRepository showingRepo,
            IShowingSeatRepository showingSeatRepo)
        {
            _showingRepo = showingRepo;
            _showingSeatRepo = showingSeatRepo;
        }

        public async Task<Result<GetSeatCount.SeatCountDto>> Handle(
            GetSeatCount.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // Validate showing exists
                var showing = await _showingRepo.GetByIdAsync(query.ShowingId, cancellationToken);
                if (showing is null)
                {
                    return Result.Failure<GetSeatCount.SeatCountDto>(
                        new Error("Showing.NotFound", $"Showing with ID '{query.ShowingId}' not found."));
                }

                // Get all showing seats for this showing
                var showingSeats = await _showingSeatRepo.GetByShowingAsync(
                    query.ShowingId, cancellationToken);

                if (showingSeats == null || !showingSeats.Any())
                {
                    return Result.Failure<GetSeatCount.SeatCountDto>(
                        new Error("ShowingSeat.NotFound", $"No seats found for showing '{query.ShowingId}'."));
                }

                // Calculate seat statistics
                var seats = showingSeats.ToList();
                var totalSeats = seats.Count;
                var availableSeats = seats.Count(ss => ss.Status.ToString() == "Available");
                var lockedSeats = seats.Count(ss => ss.Status.ToString() == "Locked");
                var bookedSeats = seats.Count(ss => ss.Status.ToString() == "Booked");

                var seatCountDto = new GetSeatCount.SeatCountDto(
                    ShowingId: query.ShowingId,
                    TotalSeats: totalSeats,
                    AvailableSeats: availableSeats,
                    LockedSeats: lockedSeats,
                    BookedSeats: bookedSeats);

                return Result.Success(seatCountDto);
            }
            catch (Exception ex)
            {
                return Result.Failure<GetSeatCount.SeatCountDto>(
                    new Error("ShowingSeat.Error",
                        $"An error occurred while fetching seat count: {ex.Message}"));
            }
        }
    }
}
