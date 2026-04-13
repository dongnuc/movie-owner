using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomsByTheater
{
    public sealed class GetRoomsByTheaterHandler
        : IQueryHandler<GetRoomsByTheater.Query, PagedResult<RoomListUserDto>>
    {
        private readonly ITheaterRepository _theaterRepo;
        private readonly IRoomRepository _roomRepo;

        public GetRoomsByTheaterHandler(ITheaterRepository theaterRepo, IRoomRepository roomRepo)
        {
            _theaterRepo = theaterRepo;
            _roomRepo = roomRepo;
        }

        public async Task<Result<PagedResult<RoomListUserDto>>> Handle(
            GetRoomsByTheater.Query query,
            CancellationToken cancellationToken)
        {
            // Verify theater exists (TheaterId is required)
            var theater = await _theaterRepo.GetByIdAsync(query.TheaterId, cancellationToken);
            if (theater is null)
                return Result.Failure<PagedResult<RoomListUserDto>>(
                    new Error("Theater.NotFound", $"Theater '{query.TheaterId}' not found."));

            var pagedRooms = await _roomRepo.GetRoomsByTheaterAsync(
                query.TheaterId,
                query.Search,
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            var response = PagedResult<RoomListUserDto>.Create(
                pagedRooms.Items.Select(GetRoomsByTheater.ToListDtoUser).ToList(),
                pagedRooms.PageIndex,
                pagedRooms.PageSize,
                pagedRooms.TotalCount);

            return Result.Success(response);
        }
    }
}
