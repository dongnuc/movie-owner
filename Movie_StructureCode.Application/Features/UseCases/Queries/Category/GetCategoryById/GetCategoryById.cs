using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategoryById
{
    public static class GetCategoryById
    {
        /// <summary>
        /// Query ?? l?y category theo ID (Admin view - ??y ?? thông tin)
        /// </summary>
        public sealed record Query(Guid Id) : IQuery<CategoryAdminDto>, ICacheableQuery
        {
            public bool BypassCache => false;
            public string CacheKey => $"category_admin_{Id}";
            public Type ValueType => typeof(CategoryAdminDto);
            public TimeSpan? Expiration => TimeSpan.FromHours(1);
        }

        /// <summary>
        /// Map Entity to User DTO (ch? Id, Name)
        /// </summary>
        public static CategoryUserDto ToDtoUser(Domain.Entities.Category c) =>
            new(c.Id, c.Name);

        /// <summary>
        /// Map Entity to Admin DTO (??y ??: Id, Name, IsActive, DateCreate, DateUpdate)
        /// </summary>
        public static CategoryAdminDto ToDtoAdmin(Domain.Entities.Category c) =>
            new(c.Id, c.Name, c.IsActive, c.DateCreate, c.DateUpdate);
    }
}
