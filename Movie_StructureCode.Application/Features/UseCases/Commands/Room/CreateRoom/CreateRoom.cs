using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Room.CreateRoom
{
    public static class CreateRoom
    {
        public sealed record Command(
            Guid    TheaterId,
            string  Name,
            int     TotalRow,
            int     TotalCol
        ) : ICommand<Guid>;
    }
}
