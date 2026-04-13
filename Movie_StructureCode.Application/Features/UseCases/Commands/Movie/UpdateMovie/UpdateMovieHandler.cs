using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Movie.UpdateMovie
{
    public sealed class UpdateMovieHandler
        : ICommandHander<UpdateMovie.Command>
    {
        private readonly IMovieRepository    _movieRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork         _uow;

        public UpdateMovieHandler(
            IMovieRepository    movieRepo,
            ICategoryRepository categoryRepo,
            IUnitOfWork         uow)
        {
            _movieRepo    = movieRepo;
            _categoryRepo = categoryRepo;
            _uow          = uow;
        }

        public async Task<Result> Handle(
            UpdateMovie.Command command,
            CancellationToken   cancellationToken)
        {
            var movie = await _movieRepo.GetByIdAsync(command.Id, cancellationToken);
            if (movie is null)
                return Result.Failure(
                    new Error("Movie.NotFound", $"Movie '{command.Id}' not found."));

            var categoryExists = await _categoryRepo.AnyAsync(
                c => c.Id == command.CategoryId && c.IsActive, cancellationToken);
            if (!categoryExists)
                return Result.Failure(
                    new Error("Movie.CategoryNotFound",
                        $"Active category '{command.CategoryId}' not found."));

            // Check if title is already used by another movie (case-insensitive, excluding current movie)
            var titleExists = await _movieRepo.AnyAsync(
                m => m.Title.ToLower() == command.Title.ToLower() && m.Id != command.Id,
                cancellationToken);
            if (titleExists)
                return Result.Failure(
                    new Error("Movie.DuplicateTitle",
                        $"Movie title '{command.Title}' already exists."));

            movie.Title       = command.Title;
            movie.Description = command.Description;
            movie.Image       = command.Image;
            movie.BasePrice   = command.BasePrice;
            movie.Duration    = command.Duration;
            movie.CategoryId  = command.CategoryId;
            movie.IsActive    = command.IsActive;
            movie.DateUpdate  = DateTime.UtcNow;

            _movieRepo.Update(movie);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
