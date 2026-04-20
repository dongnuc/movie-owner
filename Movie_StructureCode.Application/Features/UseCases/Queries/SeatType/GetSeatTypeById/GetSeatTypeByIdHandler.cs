using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Respositories;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypeById
{
    public sealed class GetSeatTypeByIdHandler
        : IQueryHandler<GetSeatTypeById.Query, SeatTypeUserDto>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;

        public GetSeatTypeByIdHandler(ISeatTypeRepository seatTypeRepo)
        {
            _seatTypeRepo = seatTypeRepo;
        }

        public async Task<Result<SeatTypeUserDto>> Handle(
            GetSeatTypeById.Query query,
            CancellationToken cancellationToken)
        {
            try
            {
                var seatType = await _seatTypeRepo.GetByIdAsync(query.Id, cancellationToken);

                if (seatType is null)
                {
                    return Result.Failure<SeatTypeUserDto>(
                        new Error("SeatType.NotFound", $"SeatType with ID '{query.Id}' not found."));
                }

                var dto = GetSeatTypeById.ToDtoUser(seatType);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<SeatTypeUserDto>(
                    new Error("SeatType.Error", $"An error occurred while retrieving seat type: {ex.Message}"));
            }
        }
    }

    public sealed class GetSeatTypeByIdAdminHandler
        : IQueryHandler<GetSeatTypeById.AdminQuery, SeatTypeAdminDto>
    {
        private readonly ISeatTypeRepository _seatTypeRepo;

        public GetSeatTypeByIdAdminHandler(ISeatTypeRepository seatTypeRepo)
        {
            _seatTypeRepo = seatTypeRepo;
        }

        public async Task<Result<SeatTypeAdminDto>> Handle(
            GetSeatTypeById.AdminQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                var seatType = await _seatTypeRepo.GetByIdAsync(query.Id, cancellationToken);

                if (seatType is null)
                {
                    return Result.Failure<SeatTypeAdminDto>(
                        new Error("SeatType.NotFound", $"SeatType with ID '{query.Id}' not found."));
                }

                var dto = GetSeatTypeById.ToDtoAdmin(seatType);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<SeatTypeAdminDto>(
                    new Error("SeatType.Error", $"An error occurred while retrieving seat type: {ex.Message}"));
            }
        }
    }
}
