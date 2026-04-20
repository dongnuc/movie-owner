using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using MediatR;
using Movie_StructureCode.Application.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Movie.DeleteMovie
{
    public sealed class DeleteMovieHandler
        : ICommandHander<DeleteMovie.Command>
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IUnitOfWork      _uow;
        private readonly IMediator        _mediator;

        public DeleteMovieHandler(IMovieRepository movieRepo, IUnitOfWork uow, IMediator mediator)
        {
            _movieRepo = movieRepo;
            _uow       = uow;
            _mediator  = mediator;
        }

        public async Task<Result> Handle(
            DeleteMovie.Command command,
            CancellationToken   cancellationToken)
        {
            var movie = await _movieRepo.GetByIdAsync(command.Id, cancellationToken);

            if (movie is null)
                return Result.Failure(
                    new Error("Movie.NotFound", $"Movie '{command.Id}' not found."));

            // Check if movie has active showings
            var hasActiveShowings = await _movieRepo.AnyAsync(
                m => m.Id == command.Id && m.Showings!.Any(s => s.IsActive),
                cancellationToken);

            if (hasActiveShowings)
                return Result.Failure(
                    new Error("Movie.HasActiveShowings",
                        "Cannot delete a movie that has active showings."));

            // Check if movie has any showings (active or inactive)
            var hasAnyShowings = await _movieRepo.AnyAsync(
                m => m.Id == command.Id && m.Showings!.Any(),
                cancellationToken);

            if (hasAnyShowings)
            {
                // Soft delete - mark as inactive (preserve data for audit trail)
                movie.IsActive = false;
                movie.DateUpdate = DateTime.UtcNow;

                _movieRepo.Update(movie);
                await _uow.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // Hard delete - remove completely (no showings at all)
                _movieRepo.Remove(movie);
                await _uow.SaveChangesAsync(cancellationToken);
            }

            // Publish cache invalidation event
            await _mediator.Publish(new EntityChangedEvent("movie", command.Id), cancellationToken);

            return Result.Success();
        }
    }
}
