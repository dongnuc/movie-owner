using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Showing.DeleteShowing
{
    public static class DeleteShowing
    {
        public sealed record Command(Guid Id) : ICommand;
    }
}
