using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using static Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSessionRemainingSeconds.GetSessionRemainingSeconds;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSessionRemainingSeconds
{
    public sealed class GetSessionRemainingSecondsHandler
        : IQueryHandler<GetSessionRemainingSeconds.Query, GetSessionRemainingSeconds.SessionRemainingSecondsDto>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly ISeatLockService _seatLockService;

        public GetSessionRemainingSecondsHandler(
            IShowingRepository showingRepo,
            ISeatLockService seatLockService)
        {
            _showingRepo = showingRepo;
            _seatLockService = seatLockService;
        }

        public async Task<Result<GetSessionRemainingSeconds.SessionRemainingSecondsDto>> Handle(
            GetSessionRemainingSeconds.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // Validate showing exists
                var showing = await _showingRepo.GetByIdAsync(query.ShowingId, cancellationToken);
                if (showing is null)
                {
                    return Result.Failure<GetSessionRemainingSeconds.SessionRemainingSecondsDto>(
                        new Error("Showing.NotFound", $"Showing with ID '{query.ShowingId}' not found."));
                }

                // Get session remaining seconds from Redis
                var remainingSeconds = await _seatLockService.GetSessionRemainingSeconds(
                    query.ShowingId, 
                    query.UserId);


                if (remainingSeconds == null)
                {
                    return Result.Failure<SessionRemainingSecondsDto>(
                        new Error("Session.Expired", "Session has expired"));
                }

                return Result.Success(new SessionRemainingSecondsDto(remainingSeconds.Value));
            }
            catch (Exception ex)
            {
                return Result.Failure<GetSessionRemainingSeconds.SessionRemainingSecondsDto>(
                    new Error("Session.Error",
                        $"An error occurred while fetching session remaining seconds: {ex.Message}"));
            }
        }
    }
}
