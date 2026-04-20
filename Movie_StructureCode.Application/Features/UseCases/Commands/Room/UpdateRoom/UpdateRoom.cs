using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Room.UpdateRoom
{
    public static class UpdateRoom
    {
        public sealed record Command(
            Guid    Id,
            string  Name,
            int TotalRow,
            int TotalCol,
            bool    IsActive
        ) : ICommand;
    }
}
