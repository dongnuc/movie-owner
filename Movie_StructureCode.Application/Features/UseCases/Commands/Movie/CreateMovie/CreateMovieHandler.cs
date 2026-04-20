using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using MediatR;
using Movie_StructureCode.Application.Common;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Movie.CreateMovie
{
    public sealed class CreateMovieHandler
        : ICommandHander<CreateMovie.Command, Guid>
    {
        private readonly IMovieRepository    _movieRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork         _uow;
        private readonly IMediator           _mediator;

        public CreateMovieHandler(
            IMovieRepository    movieRepo,
            ICategoryRepository categoryRepo,
            IUnitOfWork         uow,
            IMediator           mediator)
        {
            _movieRepo    = movieRepo;
            _categoryRepo = categoryRepo;
            _uow          = uow;
            _mediator     = mediator;
        }

        public async Task<Result<Guid>> Handle(
            CreateMovie.Command command,
            CancellationToken   cancellationToken)
        {
            // Verify category exists and is active
            var categoryExists = await _categoryRepo.AnyAsync(
                c => c.Id == command.CategoryId && c.IsActive, cancellationToken);

            if (!categoryExists)
                return Result.Failure<Guid>(
                    new Error("Movie.CategoryNotFound",
                        $"Active category '{command.CategoryId}' not found."));

            // Check if movie title already exists (case-insensitive)
            var titleExists = await _movieRepo.AnyAsync(
                m => m.Title.ToLower() == command.Title.ToLower(),
                cancellationToken);

            if (titleExists)
                return Result.Failure<Guid>(
                    new Error("Movie.DuplicateTitle",
                        $"Movie title '{command.Title}' already exists."));

            var movie = new Domain.Entities.Movie
            {
                Id          = Guid.NewGuid(),
                Title       = command.Title,
                Description = command.Description,
                Image       = command.Image,
                BasePrice   = command.BasePrice,
                Duration    = command.Duration,
                CategoryId  = command.CategoryId,
                IsActive    = true,
                DateCreate  = DateTime.UtcNow,
                DateUpdate  = DateTime.UtcNow
            };

            await _movieRepo.AddAsync(movie, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            // Publish cache invalidation event for movie list (global)
            await _mediator.Publish(new EntityChangedEvent("movie", null), cancellationToken);

            return Result.Success(movie.Id);
        }
    }
}
