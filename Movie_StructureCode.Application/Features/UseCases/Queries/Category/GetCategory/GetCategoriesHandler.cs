using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategory
{
    public sealed class GetCategoriesHandler
        : IQueryHandler<GetCategories.Query, PagedResult<CategoryUserDto>>
    {
        private readonly ICategoryRepository _repo;

        public GetCategoriesHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<CategoryUserDto>>> Handle(
            GetCategories.Query request,
            CancellationToken   cancellationToken)
        {
            var paged = await _repo.GetAllAsync(
                request.Id,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var response = PagedResult<CategoryUserDto>.Create(
                paged.Items.Select(GetCategories.ToDtoUser).ToList(),
                paged.PageIndex,
                paged.PageSize,
                paged.TotalCount);

            return Result.Success(response);
        }
    }
}
