using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Showing.UpdateShowing
{
    public static class UpdateShowing
    {
        public sealed record Command(
            Guid     Id,
            DateTime TimeStart,
            decimal  Price,
            int      MaxTicketsPerUser,
            bool     IsActive
        ) : ICommand;
    }
}
