using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.SeedData
{
    public static class SeatTypeSeedData
    {
        public static readonly Guid StandardId = new("55555555-0000-0000-0000-000000000001");
        public static readonly Guid VipId      = new("55555555-0000-0000-0000-000000000002");
        public static readonly Guid CoupleId   = new("55555555-0000-0000-0000-000000000003");

        public static IEnumerable<SeatType> GetSeedData() =>
        [
            new SeatType
            {
                Id         = StandardId,
                Name       = "Standard",
                Surcharge  = 0m,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new SeatType
            {
                Id         = VipId,
                Name       = "VIP",
                Surcharge  = 30_000m,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new SeatType
            {
                Id         = CoupleId,
                Name       = "Couple",
                Surcharge  = 50_000m,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            }
        ];
    }
}
