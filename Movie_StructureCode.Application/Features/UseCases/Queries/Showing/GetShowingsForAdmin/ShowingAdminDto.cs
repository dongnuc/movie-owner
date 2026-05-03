namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsForAdmin
{
    /// <summary>
    /// DTO cho Showing dành cho Admin - ch?a thông tin c?n thi?t
    /// </summary>
    public sealed record ShowingAdminDto(
        Guid Id,
        string MovieTitle,
        string RoomName,
        DateTime TimeStart,
        decimal Price,
        int MaxTicketsPerUser,
        bool IsActive,
        DateTime DateCreate
    );
}
