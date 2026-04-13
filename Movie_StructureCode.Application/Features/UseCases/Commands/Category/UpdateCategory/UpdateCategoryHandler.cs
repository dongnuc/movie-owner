using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Category.UpdateCategory
{
    public sealed class UpdateCategoryHandler
        : ICommandHander<UpdateCategory.Command>
    {
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork         _uow;

        public UpdateCategoryHandler(ICategoryRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        public async Task<Result> Handle(
            UpdateCategory.Command command,
            CancellationToken      cancellationToken)
        {
            var category = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (category is null)
                return Result.Failure(
                    new Error("Category.NotFound", $"Category '{command.Id}' not found."));

            // Check if name is already used by another category (case-insensitive, excluding current category)
            var isExist = await _repo.AnyAsync(c =>
                c.Name.ToLower() == command.Name.ToLower() &&
                c.Id != command.Id,
                cancellationToken);

            if (isExist)
            {
                return Result.Failure(
                    new Error("Category.DuplicateName", 
                        $"Category name '{command.Name}' already exists."));
            }

            category.Name       = command.Name;
            category.IsActive   = command.IsActive;
            category.DateUpdate = DateTime.UtcNow;

            _repo.Update(category);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
