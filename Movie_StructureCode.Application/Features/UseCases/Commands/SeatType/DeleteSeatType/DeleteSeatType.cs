using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.DeleteSeatType
{
    public static class DeleteSeatType
    {
        /// <summary>
        /// Command ?? xoá seat type
        /// </summary>
        public sealed record Command(Guid Id) : ICommand;
    }
}
