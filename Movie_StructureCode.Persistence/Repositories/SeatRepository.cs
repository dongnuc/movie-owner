using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context) { }

        // ISeatRepository không ??nh ngh?a thêm method nào,
        // toàn b? logic dùng qua IRepository<Seat> base.
    }
}
