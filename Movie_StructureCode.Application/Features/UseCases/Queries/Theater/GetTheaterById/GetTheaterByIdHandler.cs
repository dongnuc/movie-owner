using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheaterById
{
    public sealed class GetTheaterByIdHandler
        : IQueryHandler<GetTheaterById.Query, TheaterDto>
    {
        private readonly ITheaterRepository _repo;

        public GetTheaterByIdHandler(ITheaterRepository repo) => _repo = repo;

        public async Task<Result<TheaterDto>> Handle(
            GetTheaterById.Query query,
            CancellationToken cancellationToken)
        {
            var theater = await _repo.GetByIdAsync(query.Id, cancellationToken);

            if (theater is null)
                return Result.Failure<TheaterDto>(
                    new Error("Theater.NotFound", $"Theater '{query.Id}' not found."));

            return Result.Success(GetTheaterById.ToDto(theater));
        }
    }
}
