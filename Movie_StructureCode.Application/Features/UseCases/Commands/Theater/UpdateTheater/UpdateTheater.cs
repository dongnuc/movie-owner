using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Theater.UpdateTheater
{
    public static class UpdateTheater
    {
        public sealed record Command(
            Guid    Id,
            string  Name,
            string? Location,
            bool    IsActive
        ) : ICommand;
    }
}
