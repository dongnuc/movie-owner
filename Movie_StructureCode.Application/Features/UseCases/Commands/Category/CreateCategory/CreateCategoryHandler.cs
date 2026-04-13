using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Category.CreateCategory
{
    public sealed class CreateCategoryHandler
        : ICommandHander<CreateCategory.Command, Guid>
    {
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork         _uow;

        public CreateCategoryHandler(ICategoryRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        public async Task<Result<Guid>> Handle(
            CreateCategory.Command command,
            CancellationToken      cancellationToken)
        {
            var exists = await _repo.AnyAsync(
                c => c.Name == command.Name, cancellationToken);

            if (exists)
                return Result.Failure<Guid>(
                    new Error("Category.Duplicate", $"Category '{command.Name}' already exists."));

            var category = new Domain.Entities.Category
            {
                Id         = Guid.NewGuid(),
                Name       = command.Name,
                IsActive   = true,
                DateCreate = DateTime.UtcNow,
                DateUpdate = DateTime.UtcNow
            };

            await _repo.AddAsync(category, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
