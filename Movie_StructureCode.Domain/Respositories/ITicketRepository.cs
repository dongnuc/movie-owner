using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Domain.Respositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        /// <summary>
        /// Lấy ticket theo booking
        /// </summary>
        Task<IEnumerable<Ticket>> GetByBookingAsync(Guid bookingId, CancellationToken ct = default);
    }
}
