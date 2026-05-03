using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovie
{
    public sealed class GetShowingsByMovieHandler
        : IQueryHandler<GetShowingsByMovie.Query, IEnumerable<ShowingDto>>
    {
        private readonly IShowingRepository _repo;
        private readonly IShowingSeatRepository _showingSeatRepo;
        private readonly ISeatLockService _seatLockService;
        private readonly ISeatRepository _seatRepo;

        public GetShowingsByMovieHandler(IShowingRepository repo, IShowingSeatRepository showingSeatRepo, 
            ISeatLockService seatLockService, ISeatRepository seatRepo)
        {
            _repo = repo;
            _showingSeatRepo = showingSeatRepo;
            _seatLockService = seatLockService;
            _seatRepo = seatRepo;
        }

        public async Task<Result<IEnumerable<ShowingDto>>> Handle(
    GetShowingsByMovie.Query query,
    CancellationToken cancellationToken)
        {
            var showings = await _repo.GetShowingsActiveAsync(
                query.MovieId,
                query.TheaterId,
                query.Date,
                cancellationToken);

            var showingIds = showings.Select(x => x.Id).ToList();
            var roomIds = showings.Select(x => x.RoomId).Distinct().ToList();

            // 1. Lấy số ghế đã đặt (batch)
            var bookedDict = await _showingSeatRepo.CountBookedSeatsAsync(showingIds.FirstOrDefault(), cancellationToken);

            // 2. Lấy số ghế đang lock (batch Redis)
            var lockedDict = await _seatLockService.CountLockedSeatsAsync(showingIds.FirstOrDefault());

            // 3. Lấy tổng ghế theo room (batch)
            var totalSeatsDict = await _seatRepo.CountSeatUnitsByRoomIdsAsync(roomIds, cancellationToken);

            var dtos = showings.Select(showing =>
            {
                var booked = 1;
                var locked = 2;
                var totalSeats = totalSeatsDict.GetValueOrDefault(showing.RoomId);

                var available = totalSeats - (booked + locked);

                return new ShowingDto(
                    showing.Id,
                    showing.TimeStart,
                    showing.Price,
                    available
                );
            });

            return Result.Success(dtos);
        }
    }
}
