namespace Movie_StructureCode.Application.Features.UseCases.Queries.Showing
{
    /// <summary>
    /// DTO cho Showing - dùng chung cho tất cả query liên quan Showing.
    /// Nằm trong Query folder của Showing để tập trung quản lý.
    /// </summary>
    public sealed record ShowingDto(
        Guid     Id,
        DateTime TimeStart,
        decimal  Price,
        int AvaiableSeats 
    );

    /// <summary>
    /// Showings summary for a specific date - Rạp chiếu + Phòng chiếu + Số suất chiếu
    /// </summary>
    public sealed record ShowingsSummaryDto(
        Guid     TheaterId,
        string   TheaterName,
        string?  TheaterLocation,
        int      ShowingCount
    );

    /// <summary>
    /// Simple showing time info for summary
    /// </summary>
    public sealed record ShowingTimeDto(
        Guid     ShowingId,
        DateTime TimeStart,
        decimal  Price
    );
}
