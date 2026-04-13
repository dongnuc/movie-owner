using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Showing.CreateShowing
{
    public static class CreateShowing
    {
        public sealed record Command(
            Guid     MovieId,
            Guid     RoomId,
            DateTime TimeStart,
            decimal  Price,
            int      MaxTicketsPerUser
        ) : ICommand<Guid>;
    }
}
