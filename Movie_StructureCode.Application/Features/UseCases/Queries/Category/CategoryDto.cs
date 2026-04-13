namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category
{
    /// <summary>
    /// DTO cho Category dùng cho User - không c?n DateCreate, DateUpdate, IsActive
    /// </summary>
    public sealed record CategoryUserDto(
        Guid   Id,
        string Name
    );

    /// <summary>
    /// DTO cho Category dùng cho Admin - ??y ?? thông tin qu?n lý
    /// </summary>
    public sealed record CategoryAdminDto(
        Guid     Id,
        string   Name,
        bool     IsActive,
        DateTime DateCreate,
        DateTime DateUpdate
    );

    /// <summary>
    /// Legacy DTO - dùng chung (nên migrate sang User/Admin specific)
    /// </summary>
    public sealed record CategoryDto(
        Guid     Id,
        string   Name,
        bool     IsActive,
        DateTime DateCreate,
        DateTime DateUpdate
    );
}
