using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<Dictionary<Guid, int>> CountSeatUnitsByRoomIdsAsync(
        List<Guid> roomIds,
        CancellationToken cancellationToken = default);
    }
}
