using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Movie.UpdateMovie
{
    public static class UpdateMovie
    {
        public sealed record Command(
            Guid    Id,
            string  Title,
            string? Description,
            string? Image,
            decimal BasePrice,
            int     Duration,
            Guid    CategoryId,
            bool    IsActive
        ) : ICommand;
    }
}
