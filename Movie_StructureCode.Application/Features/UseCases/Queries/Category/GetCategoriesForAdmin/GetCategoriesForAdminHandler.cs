using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategoriesForAdmin
{
    public sealed class GetCategoriesForAdminHandler
        : IQueryHandler<GetCategoriesForAdmin.Query, PagedResult<CategoryAdminListDto>>
    {
        private readonly ICategoryRepository _repo;

        public GetCategoriesForAdminHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<CategoryAdminListDto>>> Handle(
            GetCategoriesForAdmin.Query request,
            CancellationToken cancellationToken)
        {
            // Get all categories
            var allCategories = await _repo.FindAsync(c => true, cancellationToken);

            // Filter by search and IsActive if specified
            var filteredItems = allCategories.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.Search))
                filteredItems = filteredItems.Where(c => 
                    c.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase));

            if (request.IsActive.HasValue)
                filteredItems = filteredItems.Where(c => c.IsActive == request.IsActive.Value);

            var totalCount = filteredItems.Count();

            var filteredList = filteredItems
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = PagedResult<CategoryAdminListDto>.Create(
                filteredList.Select(GetCategoriesForAdmin.ToDto).ToList(),
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result.Success(response);
        }
    }
}
