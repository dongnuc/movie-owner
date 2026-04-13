using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Category.DeleteCategory
{
    public sealed class DeleteCategoryHandler
        : ICommandHander<DeleteCategory.Command>
    {
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork         _uow;

        public DeleteCategoryHandler(ICategoryRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        public async Task<Result> Handle(
            DeleteCategory.Command command,
            CancellationToken      cancellationToken)
        {
            var category = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (category is null)
                return Result.Failure(
                    new Error("Category.NotFound", $"Category '{command.Id}' not found."));

            var hasMovies = await _repo.AnyAsync(
                c => c.Id == command.Id && c.Movies!.Any(), cancellationToken);

            if (hasMovies)
                return Result.Failure(
                    new Error("Category.HasMovies", "Cannot delete a category that has movies."));

            _repo.Remove(category);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
