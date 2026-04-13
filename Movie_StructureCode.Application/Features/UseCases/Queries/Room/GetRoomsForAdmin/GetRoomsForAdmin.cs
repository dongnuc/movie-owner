using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomsForAdmin
{
    public static class GetRoomsForAdmin
    {
        /// <summary>
        /// TheaterId là b?t bu?c - admin ph?i ch? ??nh r?p chi?u phim ?? xem danh sách phòng
        /// </summary>
        public sealed record Query(
            Guid    TheaterId,      // B?t bu?c - không th? null
            string? Search,
            bool?   IsActive,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<RoomListAdminDto>>;

        /// <summary>
        /// Map Entity ? Admin List DTO (compact view - ch? thông tin c? b?n)
        /// </summary>
        public static RoomListAdminDto ToListDto(Domain.Entities.Room room) =>
            new(
                room.Id,
                room.Name,
                room.TotalSeat,
                room.TotalRow,
                room.TotalCol,
                room.Theater?.Name,
                room.IsActive);

        /// <summary>
        /// Map Entity ? Admin Detail DTO (full info)
        /// </summary>
        public static RoomAdminDto ToDtoAdmin(Domain.Entities.Room room) =>
            new(
                room.Id,
                room.Name,
                room.TotalSeat,
                room.TotalRow,
                room.TotalCol,
                room.TheaterId,
                room.Theater?.Name,
                room.IsActive,
                room.DateCreate,
                room.DateUpdate);
    }
}
