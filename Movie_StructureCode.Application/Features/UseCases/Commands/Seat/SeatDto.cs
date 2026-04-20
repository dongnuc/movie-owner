namespace Movie_StructureCode.Application.Features.UseCases.Commands.Seat
{
    /// <summary>
    /// DTO cho update seat - ch?a các tr??ng c?n c?p nh?t
    /// </summary>
    public sealed record UpdateSeatDto(
        Guid Id,
        Guid SeatTypeId,
        int StatusSeat,
        Guid? GroupId
    );

    /// <summary>
    /// Response DTO cho batch update seats
    /// Tr? v? s? l??ng update thành công, th?t b?i
    /// </summary>
    public sealed record BatchUpdateSeatsResultDto(
        int SuccessCount,
        int FailureCount,
        List<string> FailureReasons
    );
}
