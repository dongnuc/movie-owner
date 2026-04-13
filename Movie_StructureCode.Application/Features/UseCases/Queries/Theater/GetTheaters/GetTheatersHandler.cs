using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheaters
{
    public sealed class GetTheatersHandler
        : IQueryHandler<GetTheaters.Query, PagedResult<TheaterDto>>
    {
        private readonly ITheaterRepository _repo;

        public GetTheatersHandler(ITheaterRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<TheaterDto>>> Handle(
            GetTheaters.Query request,
            CancellationToken cancellationToken)
        {
            var predicate = (System.Linq.Expressions.Expression<System.Func<Domain.Entities.Theater, bool>>)(t => t.IsActive);

            var theaters = await _repo.FindAsync(predicate, cancellationToken);

            var orderedTheaters = theaters
                .OrderBy(t => t.Name)
                .ToList();

            var totalCount = orderedTheaters.Count;

            var paged = orderedTheaters
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = PagedResult<TheaterDto>.Create(
                paged.Select(GetTheaters.ToDto).ToList(),
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result.Success(response);
        }
    }
}
