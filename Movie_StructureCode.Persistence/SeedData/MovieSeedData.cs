using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.SeedData
{
    public static class MovieSeedData
    {
        public static readonly Guid Movie1Id = new("22222222-0000-0000-0000-000000000001");
        public static readonly Guid Movie2Id = new("22222222-0000-0000-0000-000000000002");
        public static readonly Guid Movie3Id = new("22222222-0000-0000-0000-000000000003");
        public static readonly Guid Movie4Id = new("22222222-0000-0000-0000-000000000004");
        public static readonly Guid Movie5Id = new("22222222-0000-0000-0000-000000000005");
        public static readonly Guid Movie6Id = new("22222222-0000-0000-0000-000000000006");
        public static readonly Guid Movie7Id = new("22222222-0000-0000-0000-000000000007");

        public static IEnumerable<Movie> GetSeedData() =>
        [
            new Movie
            {
                Id          = Movie1Id,
                Title       = "The Last Stand",
                Description = "An action-packed movie about a retired sheriff who must stop a drug cartel leader.",
                Image       = "the_last_stand.jpg",
                BasePrice   = 85_000m,
                Duration    = 107,
                CategoryId  = CategorySeedData.ActionId,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            },
            new Movie
            {
                Id          = Movie2Id,
                Title       = "Midnight Fear",
                Description = "A group of friends encounter a supernatural force in an abandoned hospital.",
                Image       = "midnight_fear.jpg",
                BasePrice   = 90_000m,
                Duration    = 112,
                CategoryId  = CategorySeedData.HorrorId,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            },
            new Movie
            {
                Id          = Movie3Id,
                Title       = "Love in the City",
                Description = "A heartwarming romantic comedy set in the streets of Hanoi.",
                Image       = "love_in_the_city.jpg",
                BasePrice   = 75_000m,
                Duration    = 98,
                CategoryId  = CategorySeedData.RomanceId,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            },
            new Movie
            {
                Id          = Movie4Id,
                Title       = "Galactic Odyssey",
                Description = "Astronauts embark on a dangerous mission to the edge of the known universe.",
                Image       = "galactic_odyssey.jpg",
                BasePrice   = 100_000m,
                Duration    = 145,
                CategoryId  = CategorySeedData.SciFiId,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            },
            new Movie
            {
                Id          = Movie5Id,
                Title       = "Funny Guys",
                Description = "Two unlikely friends stumble into a series of hilarious misadventures.",
                Image       = "funny_guys.jpg",
                BasePrice   = 70_000m,
                Duration    = 95,
                CategoryId  = CategorySeedData.ComedyId,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            },
            new Movie
            {
                Id          = Movie6Id,
                Title       = "Dragon Kingdom",
                Description = "An animated adventure following a young hero destined to save the dragon realm.",
                Image       = "dragon_kingdom.jpg",
                BasePrice   = 80_000m,
                Duration    = 102,
                CategoryId  = CategorySeedData.AnimationId,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            },
            new Movie
            {
                Id          = Movie7Id,
                Title       = "Silent Witness",
                Description = "A detective races against time to uncover a serial killer before the next strike.",
                Image       = "silent_witness.jpg",
                BasePrice   = 90_000m,
                Duration    = 118,
                CategoryId  = CategorySeedData.ThrillerID,
                DateCreate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateUpdate  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive    = true
            }
        ];
    }
}
