using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Domain.Respositories;
using Movie_StructureCode.Persistence.Context;

namespace Movie_StructureCode.Persistence.Repositories
{
    public sealed class SeatTypeRepository : Repository<SeatType>, ISeatTypeRepository
    {
        public SeatTypeRepository(AppDbContext context) : base(context) { }

        // ISeatTypeRepository không ??nh ngh?a thêm method nào,
        // toàn b? logic dùng qua IRepository<SeatType> base.
    }
}
