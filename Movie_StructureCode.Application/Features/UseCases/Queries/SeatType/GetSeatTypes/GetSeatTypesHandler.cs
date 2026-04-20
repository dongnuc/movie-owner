using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypes
{
    public sealed class GetSeatTypesHandler
        : IQueryHandler<GetSeatTypes.Query, PagedResult<SeatTypeUserDto>>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;

        public GetSeatTypesHandler(ISeatTypeRepository seatTypeRepo)
        {
            _seatTypeRepo = seatTypeRepo;
        }

        public async Task<Result<PagedResult<SeatTypeUserDto>>> Handle(
            GetSeatTypes.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // L?y danh sách seat types t? database (có search + pagination)
                var seatTypes = await _seatTypeRepo.GetAllActiveSeatTypesAsync(
                    query.Search,
                    query.PageNumber,
                    query.PageSize,
                    cancellationToken);

                // Mapping to User DTO
                var response = PagedResult<SeatTypeUserDto>.Create(
                    seatTypes.Items.Select(GetSeatTypes.ToDtoUser).ToList(),
                    seatTypes.PageIndex,
                    seatTypes.PageSize,
                    seatTypes.TotalCount);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<PagedResult<SeatTypeUserDto>>(
                    new Error("SeatType.Error", $"An error occurred while retrieving seat types: {ex.Message}"));
            }
        }
    }
}
