using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetAllSeatTypesForAdmin
{
    /// <summary>
    /// Handler ?? l?y danh sách t?t c? seat types cho admin (bao g?m c? inactive)
    /// </summary>
    public sealed class GetAllSeatTypesForAdminHandler
        : IQueryHandler<GetAllSeatTypesForAdmin.Query, PagedResult<SeatTypeAdminDto>>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;

        public GetAllSeatTypesForAdminHandler(ISeatTypeRepository seatTypeRepo)
        {
            _seatTypeRepo = seatTypeRepo;
        }

        public async Task<Result<PagedResult<SeatTypeAdminDto>>> Handle(
            GetAllSeatTypesForAdmin.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                // L?y danh sách t?t c? seat types t? database (bao g?m c? inactive)
                var seatTypes = await _seatTypeRepo.GetAllSeatTypesForAdminAsync(
                    query.Search,
                    query.IsActive,
                    query.PageNumber,
                    query.PageSize,
                    cancellationToken);

                // Mapping to Admin DTO
                var response = PagedResult<SeatTypeAdminDto>.Create(
                    seatTypes.Items.Select(GetAllSeatTypesForAdmin.ToDtoAdmin).ToList(),
                    seatTypes.PageIndex,
                    seatTypes.PageSize,
                    seatTypes.TotalCount);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<PagedResult<SeatTypeAdminDto>>(
                    new Error("SeatType.Error", $"An error occurred while retrieving seat types: {ex.Message}"));
            }
        }
    }
}
