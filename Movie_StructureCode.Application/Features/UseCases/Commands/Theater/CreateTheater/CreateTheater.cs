using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Theater.CreateTheater
{
    public static class CreateTheater
    {
        public sealed record Command(
            string  Name,
            string? Location
        ) : ICommand<Guid>;
    }
}
