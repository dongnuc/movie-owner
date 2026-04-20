using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.UpdateSeatType
{
    public static class UpdateSeatType
    {
        /// <summary>
        /// Command ?? c?p nh?t seat type
        /// </summary>
        public sealed record Command(
            Guid Id,
            string Name,
            decimal Surcharge,
            bool IsActive
        ) : ICommand;
    }
}
