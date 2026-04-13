using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Category.CreateCategory
{
    public static class CreateCategory
    {
        public sealed record Command(string Name) : ICommand<Guid>;
    }
}
