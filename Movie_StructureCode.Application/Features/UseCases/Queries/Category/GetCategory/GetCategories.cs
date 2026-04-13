using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategory
{
    public static class GetCategories
    {
        public sealed record Query(
            Guid?   Id,
            int     PageNumber,
            int     PageSize
        ) : IQuery<PagedResult<CategoryUserDto>>;

        /// <summary>
        /// Map Entity ? User DTO
        /// </summary>
        public static CategoryUserDto ToDtoUser(Domain.Entities.Category c) =>
            new(c.Id, c.Name);

        /// <summary>
        /// Map Entity ? Admin DTO
        /// </summary>
        public static CategoryAdminDto ToDtoAdmin(Domain.Entities.Category c) =>
            new(c.Id, c.Name, c.IsActive, c.DateCreate, c.DateUpdate);

        /// <summary>
        /// Legacy mapper - deprecated
        /// </summary>
        public static CategoryDto ToDto(Domain.Entities.Category c) =>
            new(c.Id, c.Name, c.IsActive, c.DateCreate, c.DateUpdate);
    }
}
