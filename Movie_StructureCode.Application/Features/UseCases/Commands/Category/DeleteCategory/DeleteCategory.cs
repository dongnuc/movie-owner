using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Category.DeleteCategory
{
    public static class DeleteCategory
    {
        public sealed record Command(Guid Id) : ICommand;
    }
}
