using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.SeedData
{
    public static class RoomSeedData
    {
        // Theater 1 – CGV Vincom Center
        public static readonly Guid Room1Id = new("44444444-0000-0000-0000-000000000001");
        public static readonly Guid Room2Id = new("44444444-0000-0000-0000-000000000002");
        public static readonly Guid Room3Id = new("44444444-0000-0000-0000-000000000003");

        // Theater 2 – Lotte Cinema Landmark
        public static readonly Guid Room4Id = new("44444444-0000-0000-0000-000000000004");
        public static readonly Guid Room5Id = new("44444444-0000-0000-0000-000000000005");

        // Theater 3 – Galaxy Nguyen Du
        public static readonly Guid Room6Id = new("44444444-0000-0000-0000-000000000006");
        public static readonly Guid Room7Id = new("44444444-0000-0000-0000-000000000007");

        public static IEnumerable<Room> GetSeedData() =>
        [
            new Room
            {
                Id         = Room1Id,
                Name       = "Room 01",
                TotalSeat  = 80,
                TotalRow   = 8,
                TotalCol   = 10,
                TheaterId  = TheaterSeedData.Theater1Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Room
            {
                Id         = Room2Id,
                Name       = "Room 02",
                TotalSeat  = 60,
                TotalRow   = 6,
                TotalCol   = 10,
                TheaterId  = TheaterSeedData.Theater1Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Room
            {
                Id         = Room3Id,
                Name       = "Room 03 (IMAX)",
                TotalSeat  = 120,
                TotalRow   = 10,
                TotalCol   = 12,
                TheaterId  = TheaterSeedData.Theater1Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            // ?? Theater 2 
            new Room
            {
                Id         = Room4Id,
                Name       = "Room 01",
                TotalSeat  = 80,
                TotalRow   = 8,
                TotalCol   = 10,
                TheaterId  = TheaterSeedData.Theater2Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Room
            {
                Id         = Room5Id,
                Name       = "Room 02",
                TotalSeat  = 60,
                TotalRow   = 6,
                TotalCol   = 10,
                TheaterId  = TheaterSeedData.Theater2Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            // ?? Theater 3 
            new Room
            {
                Id         = Room6Id,
                Name       = "Room 01",
                TotalSeat  = 70,
                TotalRow   = 7,
                TotalCol   = 10,
                TheaterId  = TheaterSeedData.Theater3Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            },
            new Room
            {
                Id         = Room7Id,
                Name       = "Room 02",
                TotalSeat  = 50,
                TotalRow   = 5,
                TotalCol   = 10,
                TheaterId  = TheaterSeedData.Theater3Id,
                DateCreate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive   = true
            }
        ];
    }
}
