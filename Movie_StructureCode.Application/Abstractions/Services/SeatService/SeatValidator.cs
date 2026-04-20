using Movie_StructureCode.Application.Abstractions.Services.SeatService.InterfaceSeat;
using Movie_StructureCode.Application.Features.UseCases.Commands.Seat;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Application.Abstractions.Services.SeatService
{
    public class SeatValidator : ISeatValidator
    {
        public bool TryValidate(
        UpdateSeatDto seatDto,
        Dictionary<Guid, Seat> seatMap,
        Dictionary<Guid, SeatType> seatTypeMap,
        out string? error)
        {
            error = null;

            if (!seatMap.ContainsKey(seatDto.Id))
            {
                error = $"Seat '{seatDto.Id}' not found.";
                return false;
            }

            if (!seatTypeMap.ContainsKey(seatDto.SeatTypeId))
            {
                error = $"SeatType '{seatDto.SeatTypeId}' not found.";
                return false;
            }

            if (!Enum.IsDefined(typeof(StatusSeat), seatDto.StatusSeat))
            {
                error = $"Invalid StatusSeat '{seatDto.StatusSeat}'.";
                return false;
            }

            return true;
        }
    }
}
