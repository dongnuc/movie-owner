namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater
{
    /// <summary>
    /// DTO cho Theater dùng cho User - không c?n DateCreate, IsActive
    /// </summary>
    public sealed record TheaterUserDto(
        Guid    Id,
        string  Name,
        string? Location
    );

    /// <summary>
    /// DTO cho Theater dùng cho Admin - ??y ?? thông tin qu?n lý
    /// </summary>
    public sealed record TheaterAdminDto(
        Guid     Id,
        string   Name,
        string?  Location,
        bool     IsActive,
        DateTime DateCreate,
        DateTime DateUpdate
    );

    /// <summary>
    /// Legacy DTO - dùng chung (nên migrate sang User/Admin specific)
    /// </summary>
    public sealed record TheaterDto(
        Guid    Id,
        string  Name,
        string? Location,
        bool    IsActive,
        DateTime DateCreate
    );
}
