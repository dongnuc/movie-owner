using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Showing.DeleteShowing
{
    public sealed class DeleteShowingHandler
        : ICommandHander<DeleteShowing.Command>
    {
        private readonly IShowingRepository _showingRepo;
        private readonly IUnitOfWork _uow;

        public DeleteShowingHandler(IShowingRepository showingRepo, IUnitOfWork uow)
        {
            _showingRepo = showingRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(
            DeleteShowing.Command command,
            CancellationToken cancellationToken)
        {
            var showing = await _showingRepo.GetByIdAsync(command.Id, cancellationToken);

            if (showing is null)
                return Result.Failure(
                    new Error("Showing.NotFound", $"Showing '{command.Id}' not found."));

            showing.IsActive = false;
            showing.DateUpdate = DateTime.UtcNow;

            _showingRepo.Update(showing);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
