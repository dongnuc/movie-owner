using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomsForAdmin
{
    public sealed class GetRoomsForAdminHandler
        : IQueryHandler<GetRoomsForAdmin.Query, PagedResult<RoomListAdminDto>>
    {
        private readonly ITheaterRepository _theaterRepo;
        private readonly IRoomRepository _roomRepo;

        public GetRoomsForAdminHandler(ITheaterRepository theaterRepo, IRoomRepository roomRepo)
        {
            _theaterRepo = theaterRepo;
            _roomRepo = roomRepo;
        }

        public async Task<Result<PagedResult<RoomListAdminDto>>> Handle(
            GetRoomsForAdmin.Query request,
            CancellationToken cancellationToken)
        {
            // Verify theater exists (TheaterId is required)
            var theater = await _theaterRepo.GetByIdAsync(request.TheaterId, cancellationToken);
            if (theater is null)
                return Result.Failure<PagedResult<RoomListAdminDto>>(
                    new Error("Theater.NotFound", $"Theater '{request.TheaterId}' not found."));

            var pagedRooms = await _roomRepo.GetRoomsByTheaterAsync(
                request.TheaterId,
                request.Search,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            // Additional filter by IsActive if specified
            var filteredItems = pagedRooms.Items.AsEnumerable();

            if (request.IsActive.HasValue)
                filteredItems = filteredItems.Where(r => r.IsActive == request.IsActive.Value);

            var filteredList = filteredItems.ToList();

            var response = PagedResult<RoomListAdminDto>.Create(
                filteredList.Select(GetRoomsForAdmin.ToListDto).ToList(),
                request.PageNumber,
                request.PageSize,
                pagedRooms.TotalCount);

            return Result.Success(response);
        }
    }
}
