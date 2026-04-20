namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType
{
    /// <summary>
    /// SeatType DTO cho User - thông tin c? b?n
    /// </summary>
    public sealed record SeatTypeUserDto(
        Guid Id,
        string Name,
        decimal Surcharge
    );

    /// <summary>
    /// SeatType DTO cho Admin - ??y ?? thông tin qu?n lý
    /// </summary>
    public sealed record SeatTypeAdminDto(
        Guid Id,
        string Name,
        decimal Surcharge,
        bool IsActive,
        DateTime DateCreate,
        DateTime DateUpdate
    );

    /// <summary>
    /// Legacy SeatType DTO
    /// </summary>
    public sealed record SeatTypeDto(
        Guid Id,
        string Name,
        decimal Surcharge,
        bool IsActive,
        DateTime DateCreate,
        DateTime DateUpdate
    );
}
