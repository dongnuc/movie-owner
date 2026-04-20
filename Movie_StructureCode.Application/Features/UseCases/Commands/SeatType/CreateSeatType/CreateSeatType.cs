using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.CreateSeatType
{
    public static class CreateSeatType
    {
        /// <summary>
        /// Command ?? t?o seat type m?i
        /// </summary>
        public sealed record Command(
            string Name,
            decimal Surcharge
        ) : ICommand<Guid>;
    }
}
