using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.SeedData
{
    public static class CategorySeedData
    {
        public static readonly Guid ActionId      = new("11111111-0000-0000-0000-000000000001");
        public static readonly Guid HorrorId      = new("11111111-0000-0000-0000-000000000002");
        public static readonly Guid ComedyId      = new("11111111-0000-0000-0000-000000000003");
        public static readonly Guid RomanceId     = new("11111111-0000-0000-0000-000000000004");
        public static readonly Guid AnimationId   = new("11111111-0000-0000-0000-000000000005");
        public static readonly Guid SciFiId       = new("11111111-0000-0000-0000-000000000006");
        public static readonly Guid ThrillerID    = new("11111111-0000-0000-0000-000000000007");

        public static IEnumerable<Category> GetSeedData() =>
        [
            new Category
            {
                Id         = ActionId,
                Name       = "Action",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Category
            {
                Id         = HorrorId,
                Name       = "Horror",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Category
            {
                Id         = ComedyId,
                Name       = "Comedy",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Category
            {
                Id         = RomanceId,
                Name       = "Romance",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Category
            {
                Id         = AnimationId,
                Name       = "Animation",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Category
            {
                Id         = SciFiId,
                Name       = "Sci-Fi",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Category
            {
                Id         = ThrillerID,
                Name       = "Thriller",
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            }
        ];
    }
}
