using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomsByTheater
{
    public static class GetRoomsByTheater
    {
        /// <summary>
        /// TheaterId là b?t bu?c - ph?i ch? ??nh r?p chi?u phim ?? xem danh sách phòng
        /// </summary>
        public sealed record Query(
            Guid    TheaterId,      // B?t bu?c - không th? null
            string? Search,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<RoomListUserDto>>;

        /// <summary>
        /// Map Entity ? User List DTO (compact view - ch? thông tin c? b?n)
        /// </summary>
        public static RoomListUserDto ToListDtoUser(Domain.Entities.Room room) =>
            new(
                room.Id,
                room.Name,
                room.TotalSeat,
                room.Theater?.Name);

        /// <summary>
        /// Map Entity ? User Detail DTO (full info)
        /// </summary>
        public static RoomUserDto ToDtoUser(Domain.Entities.Room room) =>
            new(
                room.Id,
                room.Name,
                room.TotalSeat,
                room.TotalRow,
                room.TotalCol,
                room.Theater?.Name);

        /// <summary>
        /// Map Entity ? Admin Detail DTO
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

        /// <summary>
        /// Legacy mapper - deprecated
        /// </summary>
        public static RoomDto ToDto(Domain.Entities.Room room) =>
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
