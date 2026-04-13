using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheaters
{
    public static class GetTheaters
    {
        public sealed record Query(
            int PageNumber,
            int PageSize
        ) : IQuery<PagedResult<TheaterDto>>;

        public static TheaterDto ToDto(Domain.Entities.Theater t) =>
            new(t.Id, t.Name, t.Location, t.IsActive, t.DateCreate);
    }
}
