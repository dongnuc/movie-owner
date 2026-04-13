using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheatersForAdmin
{
    public sealed class GetTheatersForAdminHandler
        : IQueryHandler<GetTheatersForAdmin.Query, PagedResult<TheaterAdminListDto>>
    {
        private readonly ITheaterRepository _repo;

        public GetTheatersForAdminHandler(ITheaterRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<TheaterAdminListDto>>> Handle(
            GetTheatersForAdmin.Query request,
            CancellationToken cancellationToken)
        {
            // Get all theaters
            var allTheaters = await _repo.FindAsync(t => true, cancellationToken);

            // Filter by search and IsActive
            var filteredItems = allTheaters.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.Search))
                filteredItems = filteredItems.Where(t =>
                    t.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                    (t.Location != null && t.Location.Contains(request.Search, StringComparison.OrdinalIgnoreCase)));

            if (request.IsActive.HasValue)
                filteredItems = filteredItems.Where(t => t.IsActive == request.IsActive.Value);

            var totalCount = filteredItems.Count();

            var filteredList = filteredItems
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = PagedResult<TheaterAdminListDto>.Create(
                filteredList.Select(GetTheatersForAdmin.ToDto).ToList(),
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result.Success(response);
        }
    }
}
