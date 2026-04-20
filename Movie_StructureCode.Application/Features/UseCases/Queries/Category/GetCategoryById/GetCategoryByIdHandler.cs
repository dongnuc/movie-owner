using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategoryById
{
    /// <summary>
    /// Handler ?? x? lý query l?y category theo ID (Admin view)
    /// Ch?u trách nhi?m:
    /// - L?y category t? database theo ID
    /// - Validate category có t?n t?i
    /// - Mapping entity sang Admin DTO (??y ?? thông tin)
    /// - H? tr? caching
    /// </summary>
    public sealed class GetCategoryByIdHandler
        : IQueryHandler<GetCategoryById.Query, CategoryAdminDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// X? lý query l?y category theo ID cho admin
        /// </summary>
        /// <param name="query">Query ch?a ID c?a category c?n l?y</param>
        /// <param name="cancellationToken">Token ?? h?y operation</param>
        /// <returns>Result ch?a CategoryAdminDto (??y ?? thông tin) n?u thành công, ho?c error n?u th?t b?i</returns>
        public async Task<Result<CategoryAdminDto>> Handle(
            GetCategoryById.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // L?y category t? database theo ID
                var category = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);

                // N?u category không t?n t?i, tr? v? l?i
                if (category is null)
                {
                    return Result.Failure<CategoryAdminDto>(
                        new Error("Category.NotFound", $"Category with ID '{query.Id}' not found."));
                }

                // Mapping entity sang Admin DTO (??y ?? thông tin: Id, Name, IsActive, DateCreate, DateUpdate)
                var dto = GetCategoryById.ToDtoAdmin(category);

                // Tr? v? k?t qu? thành công
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                // X? lý exception
                return Result.Failure<CategoryAdminDto>(
                    new Error("Category.Error", $"An error occurred while retrieving category: {ex.Message}"));
            }
        }
    }
}
