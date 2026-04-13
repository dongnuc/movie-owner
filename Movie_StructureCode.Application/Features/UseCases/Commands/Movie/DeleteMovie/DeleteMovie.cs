using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Movie.DeleteMovie
{
    public static class DeleteMovie
    {
        public sealed record Command(Guid Id) : ICommand;
    }
}
