using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheatersForAdmin
{
    public static class GetTheatersForAdmin
    {
        public sealed record Query(
            string? Search,
            bool?   IsActive,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<TheaterAdminListDto>>;

        /// <summary>
        /// Map Entity ? Admin List DTO (compact view for table)
        /// </summary>
        public static TheaterAdminListDto ToDto(Domain.Entities.Theater t) =>
            new(t.Id, t.Name, t.Location, t.IsActive);
    }
}
