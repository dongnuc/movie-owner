using Movie_StructureCode.Application.Features.UseCases.Commands.Seat;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat
{
    public interface ISeatValidator
    {
        bool TryValidate(
        UpdateSeatDto seatDto,
        Dictionary<Guid, Seat> seatMap,
        Dictionary<Guid, SeatType> seatTypeMap,
        out string? error);
    }
}
