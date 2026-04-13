using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.SeedData
{
    public static class TheaterSeedData
    {
        public static readonly Guid Theater1Id = new("33333333-0000-0000-0000-000000000001");
        public static readonly Guid Theater2Id = new("33333333-0000-0000-0000-000000000002");
        public static readonly Guid Theater3Id = new("33333333-0000-0000-0000-000000000003");

        public static IEnumerable<Theater> GetSeedData() =>
        [
            new Theater
            {
                Id         = Theater1Id,
                Name       = "CGV Vincom Center",
                Location   = "72 Le Thanh Ton, Ben Nghe, District 1, Ho Chi Minh City",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Theater
            {
                Id         = Theater2Id,
                Name       = "Lotte Cinema Landmark",
                Location   = "772B Dien Bien Phu, Ward 22, Binh Thanh, Ho Chi Minh City",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Theater
            {
                Id         = Theater3Id,
                Name       = "Galaxy Nguyen Du",
                Location   = "116 Nguyen Du, Ben Thanh, District 1, Ho Chi Minh City",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            }
        ];
    }
}
