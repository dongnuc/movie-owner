using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Category.UpdateCategory
{
    public static class UpdateCategory
    {
        public sealed record Command(
            Guid   Id,
            string Name,
            bool   IsActive
        ) : ICommand;
    }
}
