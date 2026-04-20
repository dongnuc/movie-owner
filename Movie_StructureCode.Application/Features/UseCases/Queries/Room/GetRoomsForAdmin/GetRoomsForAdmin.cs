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
        public static RoomListAdminDto ToListDto(Movie_StructureCode.Domain.Entities.Room room, int totalSeat)
        {
            return new RoomListAdminDto(
                room.Id,
                room.Name,
                totalSeat,
                room.TotalRow,
                room.TotalCol,
                room.IsActive
            );
        }

    }
}
