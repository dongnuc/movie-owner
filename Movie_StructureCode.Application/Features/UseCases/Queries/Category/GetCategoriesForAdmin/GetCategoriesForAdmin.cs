using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategoriesForAdmin
{
    public static class GetCategoriesForAdmin
    {
        public sealed record Query(
            string? Search,
            bool?   IsActive,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<CategoryAdminListDto>>;

        /// <summary>
        /// Map Entity ? Admin List DTO (compact view for table)
        /// </summary>
        public static CategoryAdminListDto ToDto(Domain.Entities.Category c) =>
            new(c.Id, c.Name, c.IsActive);
    }
}
