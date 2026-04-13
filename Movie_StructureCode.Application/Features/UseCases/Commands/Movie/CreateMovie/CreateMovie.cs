using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Movie.CreateMovie
{
    public static class CreateMovie
    {
        public sealed record Command(
            string  Title,
            string? Description,
            string? Image,
            decimal BasePrice,
            int     Duration,
            Guid    CategoryId
        ) : ICommand<Guid>;
    }
}
