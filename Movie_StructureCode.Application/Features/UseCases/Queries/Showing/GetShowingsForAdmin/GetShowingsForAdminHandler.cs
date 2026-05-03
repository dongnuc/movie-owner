using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsForAdmin
{
    public sealed class GetShowingsForAdminHandler
        : IQueryHandler<GetShowingsForAdmin.Query, PagedResult<ShowingAdminDto>>
    {
        private readonly IShowingRepository _showingRepo;

        public GetShowingsForAdminHandler(IShowingRepository showingRepo)
        {
            _showingRepo = showingRepo;
        }

        public async Task<Result<PagedResult<ShowingAdminDto>>> Handle(
            GetShowingsForAdmin.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // STEP 1: L?y danh sách showing theo theater v?i các filter
                var showingsPagedResult = await _showingRepo.GetShowingsForAdminByTheaterAsync(
                    query.TheaterId,
                    query.MovieTitle,
                    query.TimeStart,
                    query.IsActive,
                    query.PageNumber,
                    query.PageSize,
                    cancellationToken);

                if (!showingsPagedResult.Items.Any())
                {
                    return Result.Success(PagedResult<ShowingAdminDto>.Create(
                        new List<ShowingAdminDto>(),
                        query.PageNumber,
                        query.PageSize,
                        0));
                }

                // STEP 2: Mapping to Admin DTO v?i thông tin c?n thi?t
                var dtos = showingsPagedResult.Items.Select(showing => new ShowingAdminDto(
                    Id: showing.Id,
                    MovieTitle: showing.Movie?.Title ?? "N/A",
                    RoomName: showing.Room?.Name ?? "N/A",
                    TimeStart: showing.TimeStart,
                    Price: showing.Price,
                    MaxTicketsPerUser: showing.MaxTicketsPerUser,
                    IsActive: showing.IsActive,
                    DateCreate: showing.DateCreate
                )).ToList();

                var result = PagedResult<ShowingAdminDto>.Create(
                    dtos,
                    query.PageNumber,
                    query.PageSize,
                    showingsPagedResult.TotalCount);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<PagedResult<ShowingAdminDto>>(
                    new Error("Showing.Error",
                        $"An error occurred while fetching showings: {ex.Message}"));
            }
        }
    }
}
