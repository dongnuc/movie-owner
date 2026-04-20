namespace Movie_StructureCode.Application.Features.UseCases.Queries.Room
{
    public enum SeatBookingStatusEnum
    {
        Available = 1,
        Booked = 2,
        Holding = 3,
    }

    public enum SeatConfigStatusEnum
    {
        Active = 1,
        Broken =2,
        None = 3,
    }

    public sealed record SeatStatusDto
    (
        Guid SeatId,
            SeatBookingStatusEnum StatusBook,
            Guid SeatTypeId,
            Guid? GroupId
    );
    // room
    public sealed record RoomListUserDto
    (
        Guid Id,
        string Name,
        int TotalSeat
    );

    public sealed record RoomListAdminDto
    (
        Guid Id,
        string Name,
        int TotalSeat,
        int TotalRow,
        int TotalCol,
        bool IsActive
    );

    // seat and room Detail
    public sealed record SeatLayoutUserDto
    (
        Guid SeatId,
        int RowIndex,
        int ColIndex,
        string RowName,
        int SeatNumber,
        Guid SeatTypeId,
        string SeatTypeName,
        decimal SeatTypeSurcharge
        );

    public sealed record SeatRowLayoutUserDto
    (
        int RowIndex,
        string RowName,
        IEnumerable<SeatLayoutUserDto> Seats
    );
    public sealed record SeatRowLayoutAdminDto
(
    int RowIndex,
    string RowName,
    IEnumerable<SeatLayoutAdminDto> Seats
);

    public sealed record SeatLayoutAdminDto
    (
        Guid? GroupId,
        string Type,
        string RowName,
        int RowIndex,
        int StartColIndex,
        int DisplayNumber,
        List<Guid> SeatIds,
        Guid SeatTypeId,
        string SeatTypeName,
        SeatConfigStatusEnum StatusSeat
        );

    public sealed record RoomDetailUserDto
    (
        Guid RoomId,
        IEnumerable<SeatRowLayoutUserDto> Layout,
        IEnumerable<SeatStatusDto> StatusSeats
    );

    public sealed record RoomDetailAdminDto
    (
        Guid Id,
        string Name,
        int TotalSeat,
        int TotalRow,
        int TotalCol,
        bool IsActive,
        DateTime DateCreate,
        DateTime DateUpdate,
        IEnumerable<SeatRowLayoutAdminDto>? Layout
    );



}
