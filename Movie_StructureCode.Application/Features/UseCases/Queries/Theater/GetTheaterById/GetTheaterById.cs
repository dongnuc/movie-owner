using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheaterById
{
    public static class GetTheaterById
    {
        public sealed record Query(Guid Id) : IQuery<TheaterDto>;

        public static TheaterDto ToDto(Domain.Entities.Theater t) =>
            GetTheaters.GetTheaters.ToDto(t);
    }
}
