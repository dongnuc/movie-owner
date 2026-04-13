using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.SeedData
{
    public static class ShowingSeedData
    {
        public static readonly Guid Showing1Id  = new("66666666-0000-0000-0000-000000000001");
        public static readonly Guid Showing2Id  = new("66666666-0000-0000-0000-000000000002");
        public static readonly Guid Showing3Id  = new("66666666-0000-0000-0000-000000000003");
        public static readonly Guid Showing4Id  = new("66666666-0000-0000-0000-000000000004");
        public static readonly Guid Showing5Id  = new("66666666-0000-0000-0000-000000000005");
        public static readonly Guid Showing6Id  = new("66666666-0000-0000-0000-000000000006");
        public static readonly Guid Showing7Id  = new("66666666-0000-0000-0000-000000000007");
        public static readonly Guid Showing8Id  = new("66666666-0000-0000-0000-000000000008");
        public static readonly Guid Showing9Id  = new("66666666-0000-0000-0000-000000000009");
        public static readonly Guid Showing10Id = new("66666666-0000-0000-0000-000000000010");

        public static IEnumerable<Showing> GetSeedData() =>
        [
            // ?? Room 1 ??????????????????????????????????????????????????????
            new Showing
            {
                Id                 = Showing1Id,
                TimeStart          = new DateTime(2025, 6, 1,  9, 0, 0, DateTimeKind.Utc),
                Price              = 85_000m,
                MaxTicketsPerUser  = 8,
                MovieId            = MovieSeedData.Movie1Id,
                RoomId             = RoomSeedData.Room1Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            new Showing
            {
                Id                 = Showing2Id,
                TimeStart          = new DateTime(2025, 6, 1, 13, 0, 0, DateTimeKind.Utc),
                Price              = 85_000m,
                MaxTicketsPerUser  = 8,
                MovieId            = MovieSeedData.Movie1Id,
                RoomId             = RoomSeedData.Room1Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            new Showing
            {
                Id                 = Showing3Id,
                TimeStart          = new DateTime(2025, 6, 1, 17, 0, 0, DateTimeKind.Utc),
                Price              = 90_000m,
                MaxTicketsPerUser  = 8,
                MovieId            = MovieSeedData.Movie2Id,
                RoomId             = RoomSeedData.Room1Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            // ?? Room 2 ??????????????????????????????????????????????????????
            new Showing
            {
                Id                 = Showing4Id,
                TimeStart          = new DateTime(2025, 6, 1, 10, 0, 0, DateTimeKind.Utc),
                Price              = 75_000m,
                MaxTicketsPerUser  = 6,
                MovieId            = MovieSeedData.Movie3Id,
                RoomId             = RoomSeedData.Room2Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            new Showing
            {
                Id                 = Showing5Id,
                TimeStart          = new DateTime(2025, 6, 1, 14, 0, 0, DateTimeKind.Utc),
                Price              = 100_000m,
                MaxTicketsPerUser  = 6,
                MovieId            = MovieSeedData.Movie4Id,
                RoomId             = RoomSeedData.Room2Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            // ?? Room 3 (IMAX) ????????????????????????????????????????????????
            new Showing
            {
                Id                 = Showing6Id,
                TimeStart          = new DateTime(2025, 6, 1, 11, 0, 0, DateTimeKind.Utc),
                Price              = 120_000m,
                MaxTicketsPerUser  = 10,
                MovieId            = MovieSeedData.Movie4Id,
                RoomId             = RoomSeedData.Room3Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            new Showing
            {
                Id                 = Showing7Id,
                TimeStart          = new DateTime(2025, 6, 1, 15, 30, 0, DateTimeKind.Utc),
                Price              = 120_000m,
                MaxTicketsPerUser  = 10,
                MovieId            = MovieSeedData.Movie7Id,
                RoomId             = RoomSeedData.Room3Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            // ?? Room 4 (Theater 2) ???????????????????????????????????????????
            new Showing
            {
                Id                 = Showing8Id,
                TimeStart          = new DateTime(2025, 6, 2,  9, 0, 0, DateTimeKind.Utc),
                Price              = 80_000m,
                MaxTicketsPerUser  = 8,
                MovieId            = MovieSeedData.Movie5Id,
                RoomId             = RoomSeedData.Room4Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            new Showing
            {
                Id                 = Showing9Id,
                TimeStart          = new DateTime(2025, 6, 2, 13, 0, 0, DateTimeKind.Utc),
                Price              = 80_000m,
                MaxTicketsPerUser  = 8,
                MovieId            = MovieSeedData.Movie6Id,
                RoomId             = RoomSeedData.Room4Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            },
            // ?? Room 6 (Theater 3) ???????????????????????????????????????????
            new Showing
            {
                Id                 = Showing10Id,
                TimeStart          = new DateTime(2025, 6, 2, 19, 0, 0, DateTimeKind.Utc),
                Price              = 90_000m,
                MaxTicketsPerUser  = 5,
                MovieId            = MovieSeedData.Movie7Id,
                RoomId             = RoomSeedData.Room6Id,
                DateCreate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate         = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive           = true
            }
        ];
    }
}
