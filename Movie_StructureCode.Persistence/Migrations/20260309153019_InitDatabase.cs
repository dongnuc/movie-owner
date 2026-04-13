using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movie_StructureCode.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surcharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theater", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUsedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpriseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movie_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalSeat = table.Column<int>(type: "int", nullable: false),
                    TotalRow = table.Column<int>(type: "int", nullable: false),
                    TotalCol = table.Column<int>(type: "int", nullable: false),
                    TheaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Theater_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "Theater",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowIndex = table.Column<int>(type: "int", nullable: false),
                    ColIndex = table.Column<int>(type: "int", nullable: false),
                    RowName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusSeat = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    SeatTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seat_SeatType_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Showing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxTicketsPerUser = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Showing_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Showing_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusBooking = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booking_Showing_ShowingId",
                        column: x => x.ShowingId,
                        principalTable: "Showing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TicketStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Seat_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "DateCreate", "DateUpdate", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Action" },
                    { new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Horror" },
                    { new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Comedy" },
                    { new Guid("11111111-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Romance" },
                    { new Guid("11111111-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Animation" },
                    { new Guid("11111111-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sci-Fi" },
                    { new Guid("11111111-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Thriller" }
                });

            migrationBuilder.InsertData(
                table: "SeatType",
                columns: new[] { "Id", "DateCreate", "DateUpdate", "IsActive", "Name", "Surcharge" },
                values: new object[,]
                {
                    { new Guid("55555555-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Standard", 0m },
                    { new Guid("55555555-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "VIP", 30000m },
                    { new Guid("55555555-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Couple", 50000m }
                });

            migrationBuilder.InsertData(
                table: "Theater",
                columns: new[] { "Id", "DateCreate", "DateUpdate", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("33333333-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "72 Le Thanh Ton, Ben Nghe, District 1, Ho Chi Minh City", "CGV Vincom Center" },
                    { new Guid("33333333-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "772B Dien Bien Phu, Ward 22, Binh Thanh, Ho Chi Minh City", "Lotte Cinema Landmark" },
                    { new Guid("33333333-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "116 Nguyen Du, Ben Thanh, District 1, Ho Chi Minh City", "Galaxy Nguyen Du" }
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "BasePrice", "CategoryId", "DateCreate", "DateUpdate", "Description", "Duration", "Image", "IsActive", "Title" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000001"), 85000m, new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An action-packed movie about a retired sheriff who must stop a drug cartel leader.", 107, "the_last_stand.jpg", true, "The Last Stand" },
                    { new Guid("22222222-0000-0000-0000-000000000002"), 90000m, new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A group of friends encounter a supernatural force in an abandoned hospital.", 112, "midnight_fear.jpg", true, "Midnight Fear" },
                    { new Guid("22222222-0000-0000-0000-000000000003"), 75000m, new Guid("11111111-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A heartwarming romantic comedy set in the streets of Hanoi.", 98, "love_in_the_city.jpg", true, "Love in the City" },
                    { new Guid("22222222-0000-0000-0000-000000000004"), 100000m, new Guid("11111111-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Astronauts embark on a dangerous mission to the edge of the known universe.", 145, "galactic_odyssey.jpg", true, "Galactic Odyssey" },
                    { new Guid("22222222-0000-0000-0000-000000000005"), 70000m, new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Two unlikely friends stumble into a series of hilarious misadventures.", 95, "funny_guys.jpg", true, "Funny Guys" },
                    { new Guid("22222222-0000-0000-0000-000000000006"), 80000m, new Guid("11111111-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An animated adventure following a young hero destined to save the dragon realm.", 102, "dragon_kingdom.jpg", true, "Dragon Kingdom" },
                    { new Guid("22222222-0000-0000-0000-000000000007"), 90000m, new Guid("11111111-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A detective races against time to uncover a serial killer before the next strike.", 118, "silent_witness.jpg", true, "Silent Witness" }
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "Id", "DateCreate", "DateUpdate", "IsActive", "Name", "TheaterId", "TotalCol", "TotalRow", "TotalSeat" },
                values: new object[,]
                {
                    { new Guid("44444444-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 01", new Guid("33333333-0000-0000-0000-000000000001"), 10, 8, 80 },
                    { new Guid("44444444-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 02", new Guid("33333333-0000-0000-0000-000000000001"), 10, 6, 60 },
                    { new Guid("44444444-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 03 (IMAX)", new Guid("33333333-0000-0000-0000-000000000001"), 12, 10, 120 },
                    { new Guid("44444444-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 01", new Guid("33333333-0000-0000-0000-000000000002"), 10, 8, 80 },
                    { new Guid("44444444-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 02", new Guid("33333333-0000-0000-0000-000000000002"), 10, 6, 60 },
                    { new Guid("44444444-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 01", new Guid("33333333-0000-0000-0000-000000000003"), 10, 7, 70 },
                    { new Guid("44444444-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Room 02", new Guid("33333333-0000-0000-0000-000000000003"), 10, 5, 50 }
                });

            migrationBuilder.InsertData(
                table: "Seat",
                columns: new[] { "Id", "ColIndex", "DateCreate", "LastUpdated", "RoomId", "RowIndex", "RowName", "SeatNumber", "SeatTypeId" },
                values: new object[,]
                {
                    { new Guid("00cc7c56-3584-4f7b-9731-482d00ceba4c"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("010d8820-d24e-455e-aae6-b213ecd0b6ae"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("015a191b-2c93-4516-860d-d63eef39db55"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0221eece-380c-45c9-9202-3c2bc38b6f8e"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0308619e-9a09-46b5-8342-921aeedbc5e1"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("03ce72ef-9d9d-46c1-b5de-23072936ac58"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("04dbec81-1ddb-47d0-bfd0-dfe69f2d7526"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("051c0526-6e3a-4b4a-ae21-4ef405a16742"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0632c324-d296-4a95-bbdd-9282fb466122"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("06801c5f-e330-41e8-940d-04d82faf4c59"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("07804793-dad5-4bf6-95f2-4993fd166e1a"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("081fa84c-4c4c-45d7-9ad5-0b4787c47979"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("099d00fb-a921-43f7-a8cb-77e2c75f9e74"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("09b93a92-1a5f-432f-8598-bbcc6751310a"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0a36b1b6-7cb4-4f98-88cd-c8d0ee85d2d9"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0afc4484-d977-4f9f-ae6b-8a1e724445b2"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0b684255-6adb-4a5d-97f4-0c9a03f036bd"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0de602a5-2cd6-49df-8a82-a9b138c1fa5c"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0e2eb286-842d-4d4e-b05b-57512f756ea5"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("0fcdb49b-15fb-4691-9f6a-82d1142cbab9"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 10, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("102e9549-2a62-4f77-afc6-76bcfdb9dbc5"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("11d64c4b-0f51-4651-ac37-7b7094acdcd0"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("148ce105-a0ca-4da0-86d6-12e37685a8c9"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("156adc56-b8c7-4d59-9d9b-4ca430145345"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("165997d5-2278-471c-96cc-9f913956b5fa"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("17a57a56-440e-40c7-ad64-20158f515d56"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("17d2f599-1d6d-48de-b545-31291f24ee8b"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 10, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("1930c405-8b77-4e41-9863-8841d0b0ad53"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 9, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("1a1abce4-c65e-4721-b5fe-6b68381d57ab"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 1, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("1aa10c88-261d-4c84-92c9-03ce6606f775"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 3, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("1aaa0b67-3da1-4ef2-a0b6-eba25a1090d7"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("1ba035d6-7df4-4cec-98aa-282e37ed1c76"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("1ce95e3b-c54f-4dc5-b591-0c012bc94e1f"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 6, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("1f1318d7-9849-4a55-b07e-8a10531622a2"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("200862a6-04c3-4750-a6f3-f6e29c231c85"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("20b68b73-84a6-41a9-804c-b6f442f2d3d7"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("2139ca01-acc8-40db-80c3-a1da5c1367e6"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("219bcbe4-5c54-4fa2-b25b-6a3a2c17e303"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 7, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("24940133-25f5-4dc1-8816-7d9d8ed00dcf"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 3, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("25974ae0-e750-4a28-8081-2fb827deaf6b"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("26e50ab5-0889-48e5-983d-9325fabf0914"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("27332140-3f81-48dd-92d5-6805594d042f"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("282d2134-70fa-40dd-a683-c8b70cff7a9d"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("28db05ff-b6d6-4a31-9ea0-75e9f219c8f4"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 6, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("2a6ac753-e2bf-4954-9e93-2ec91e58869c"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("2b0f63ea-b736-46d1-86bf-60c87c4def43"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("2cb5602a-a9ef-480b-9ad8-45822e9e6512"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 6, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("300074ba-4b15-4bce-b9fb-4fb1b6a66e50"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("30c69fe6-7300-4746-b18b-722e49e03f0b"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("325e9a50-3ec7-403f-8bb1-f73b6c07f0da"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 9, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("34014e58-b6a3-4d0e-ba24-46c436f4df39"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("34939af2-7140-4d7c-babc-69823628491f"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 8, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("34fef281-ec9a-443a-87b7-f085c8b1daa6"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("3746aff8-f543-48cc-a10a-b08939142f3d"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("379cf25b-5590-49e8-88de-342c147d3766"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("37a72b0e-c918-49e9-9015-93b244bae8fb"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("380756cd-c9ca-4a0c-8e51-f88739388b54"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 5, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("3856ac1f-9ab9-4a6e-a060-2c5d27260828"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 5, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("398cbe8d-fc4c-4381-ade8-c15f517dfaab"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 3, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("3b893721-c3f0-44b9-863b-5d129bab3c05"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("3cec3fd6-e6b0-40a8-a2d4-bb82c2a92154"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("422817d0-9139-4c6e-a849-82958bbc6562"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 7, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("426f1677-0329-47d7-bd12-9dcbc790631d"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 5, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("46072e4b-0bdf-4c65-939f-8d6f59b254f3"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("46878e4f-126c-46f7-ad7e-76735fc14769"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("4c368a85-7fac-4277-a824-dc85db9f1695"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("4c815c22-99ec-4623-9c8f-da363194ccb4"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("4cc5a32b-6ccf-4ab3-8027-ea4569b42418"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("4d3517e6-4bb4-48da-bc4b-d1aa68bd7fe1"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("4f28a217-4b48-406d-b7c0-9cad7a219777"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("50936775-98d6-4984-8370-d76f583323ec"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("50e03a3a-ad1f-4abb-baf8-8458e0211660"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("527bf9d0-9994-4493-a99a-2429f98cdac6"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 8, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("537e67ee-78c5-4ed5-84e0-6f933514f0d3"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 6, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("53ba1ef4-81ec-42fd-8cb3-84a58f821ab4"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("53cb1753-119a-4aeb-b56c-406b375e091e"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("5436347b-c755-4b31-a13f-e9cc3deb8fdc"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("55ebfee1-54d2-4eaa-9f34-a5a9a9dac365"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("562f8bce-f1a6-4501-9dc7-7d01d0c677ff"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("56e2bb02-858d-4c82-8580-7a243bb9bc3e"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 3, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("571f2a09-8f22-4f9b-b4ca-d27bb01a4ad4"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 1, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("57fc8c13-ebfc-4472-bb93-6b4895a95dff"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 6, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("58754496-078d-4f35-a35a-924eb071d4c1"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 1, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("59c01b7f-8f05-40a2-a361-5e673831e3d8"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 4, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("5b652a07-081b-4fec-8da0-c239a9e68776"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("5b6badcf-9f13-4128-8692-cf29abaa249b"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("5d1c8a92-a1ec-4b83-a2f0-74f08920ac44"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 9, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("5d446ebf-a04b-4273-8694-b3753f66f3b3"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("5f80b536-12b9-4b7d-b603-59aae4207b5b"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("60b0ef65-e4af-4e35-8c43-44461e446de6"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 1, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("60caaae2-5a69-461a-87e3-38f2e5bed007"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("6150f2a2-fa69-4a62-ab98-06ee0ab28f51"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("629ad319-11e8-475c-871f-10758e3128e5"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 7, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("62c978e4-4889-4d06-89be-30e7ff5c06f3"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 5, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("638b0874-484f-432f-ae55-2a98a4b85490"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("66bb8535-7331-49a6-9f46-ce23d29741ca"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("66cf9f8d-997c-4571-b255-e18f67f97578"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("6a27a350-85e3-439b-8c14-fb1ede742e0a"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("6c352f89-cf41-4e20-9b75-82814935e6a7"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 8, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("6fdb2ebd-0dcc-4ff7-8cad-27b7207e3e0e"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 2, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("7414bb7a-6117-450e-bd75-99fdf78762e3"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("77f4432d-1eb1-4dff-b8f7-7c4195dfd9c4"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("78fb559a-c888-4207-9905-879babe7083c"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("792a12f4-827f-4e48-8b67-f50ff200d034"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 1, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("7a632dbb-639c-4e5a-a128-13f2746cc968"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("7b41ea95-56eb-4634-9b41-3b935bbd76a1"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 5, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("7bb64eda-5d87-4e3a-805a-4d04548cfffd"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 2, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("7e19c903-35e3-4a5c-8175-2b4b57bf5812"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("7e2ad5fe-5d60-4a22-8051-f7786761b77f"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("7ffe2555-d7d8-48c4-93cf-1902530b1f25"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("8230969b-fe3f-466d-8641-06a1de95137f"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 4, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("827abd22-c2f4-4cca-970d-593655378fc5"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 3, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("87b2891b-020c-4ae7-a398-129f21e94020"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 1, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("880bc117-175e-47b3-b015-c187b5104a65"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("886eb8e1-818b-4384-8c91-43ec74cbd9e2"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("8a01dc12-3621-4c11-a360-203b0ab57d0e"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("8b8bb4e8-b90c-4e67-b75e-289993dcc2bb"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("8c5f232d-975d-480c-9b70-2274fe639563"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 4, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("8cb0b6a7-636d-4523-a090-ffd7e5eba793"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("8d29a064-c969-4259-9a54-0aba6a64a22e"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 2, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("8f9e13c1-f1d6-4f2e-bda7-258ba4e40965"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 8, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("8fa42fe6-bdee-4824-839c-b6a1950cd790"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("9057ef7b-23bc-4e05-9032-63018f91a9d4"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("90e15df6-e13c-4de7-a657-37763e627245"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("913dd03b-ab85-41ac-8fc8-17570ad24055"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("92aaf7ab-3ef9-44ba-8e81-93e2f7db7977"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("930160fa-fbac-4fdf-bcb7-ae99c71431ef"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 6, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("933b2f92-1a2a-4889-8e6f-e8c5e0a825f5"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 2, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("947c927d-8ae2-4492-8f15-810c6e5b1aba"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("94aab969-b9fc-4b6b-b3f5-0c0e5b79197c"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 4, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("94aee7db-fdb3-4a4f-93b6-8f176f9948a2"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 10, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("950e691b-4509-496e-ac67-2856cd4831bd"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("9593b18c-eccf-471e-98d8-9938b6045aa6"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("97ad342a-29b9-4f51-af52-fafbfa38cb90"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 3, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("989c38b7-0c66-4852-bf34-1265911d6244"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 4, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("9bba5b51-9a81-485e-9595-caa6a0baf914"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("9c0e6e31-dba2-431f-b87e-86239f194e65"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("9e61127f-3fdf-45c6-9e8c-6ee275286310"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("9e6180cd-70fa-436e-8073-3c34d4446767"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 5, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("9f2505f1-5138-422f-a983-52d49cb380cb"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("9f8ea4f2-a4b3-4bed-877a-a559c1f3c31b"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 3, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("9f90694f-5b95-46f7-a905-79b7575d2505"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("a1ebbc6c-2d1a-433e-b8cf-a33a87ffc536"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 4, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("a26c9436-b194-4122-88a3-aa6f8d1cba73"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 9, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("a351a0d7-78e0-428b-870b-d8517f9b5d68"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 9, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("a44d7535-f247-4ca7-abee-140f2084afbd"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("a46ad56c-3804-4fd9-a233-971d5cc97a35"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("a955fda7-b8e9-4f72-8ab8-d8fd833918e2"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("ac3fb3d0-2e8d-4121-b6fd-b33841f910c1"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 5, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("ac4a334e-e402-4ffe-a739-4235c15f3ce8"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("aeea2732-df5c-492a-a46b-7c9dd13d3a04"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 7, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("af11115f-b6aa-4100-b749-2f2c7f39f818"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 10, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("b0f33484-84d5-44d9-8ef1-deeb4750ed07"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b1305c2b-07b1-4059-ab53-43c0e85e7064"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 4, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("b14689c7-7f81-4848-9739-6d8667568191"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b1ca9430-4954-4c8e-b0ff-d1abb0fe2c40"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b1f7f317-e483-4ac1-8f52-9d11e640fb05"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b2e76b39-9441-44cf-9f13-730aa9f5995d"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b39f29d1-6fd4-4664-9234-4ea82b490873"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b44849cb-c107-42bd-81e5-a4382eb897df"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b4e98099-aa82-42b3-ba81-6b0e4016dc3b"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b61957a7-78a8-4b94-bd3c-edf174ae6210"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b66333f8-bd06-4109-abb4-f7505584bb5c"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("b7bb8ba3-6a2a-4a85-ba11-3a59c903d2e2"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 2, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("bc200178-4b75-4cd2-a141-cb48de4b00d3"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("c0734283-4f43-4a1f-aea4-f9518023920f"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 1, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("c0cebdcc-02b7-452b-bd30-5e958f10304f"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("c10df3d2-45df-4f42-88b6-8d3022d53b8a"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 8, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("c1150702-ee5e-4b0c-8e8d-dd16144058c0"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("c13f0732-7e1a-4f28-8ef9-82dd63de3629"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 1, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("c379fedd-2525-45b4-8473-2e83d7b26e9d"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 0, "A", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("c4477aa1-13f5-4d1e-b723-e19d9fe776f5"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("c61218f5-5d9a-4d05-9e98-fab04339854b"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 7, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("c77a9470-c7cc-433d-8457-7139f1a5f91f"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 8, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("c8bdc584-eb08-457d-b6a6-8d3acd3df8e6"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 2, "C", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("c946da11-4ed2-4589-a9ca-cf6d8179d549"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("cc25f06e-91ba-4cc2-be51-29294ffe1659"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 10, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("ce40edd3-d240-4f7f-8622-1042668f037b"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 2, "C", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("d115d531-b15a-4888-a351-796d19d8b892"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 5, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("d1c00c06-74ee-457b-9775-680020f7c189"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 3, "D", 6, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("d31523d5-656b-43a9-80b1-5675f8e0d48f"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("d3e8c186-6be0-48a0-aa61-82e5d23fd9d2"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("d53c0438-b297-41c1-8472-a4051f73e64d"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 2, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("d6f6a134-79fb-468e-84ef-b29af00833d2"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("d85491aa-90fa-4c62-ac00-18d2a785b8c1"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 8, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("d9466c2c-428e-4595-bfc1-79adb1991414"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 6, "G", 7, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("da00b04f-9e03-48af-a86e-4199e513a839"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 7, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("da791c9f-9daf-4e9f-ba0e-cad911e858d0"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 1, "B", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("db720286-7fdb-49bb-ac4d-bb9b373d7fe7"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 9, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("dc9d071d-1ed9-487f-aef0-8fbc32dd47e1"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("ddf5ab25-cb92-4b76-92e4-9f0a6cf84dd8"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("de085b04-5b62-4596-b051-7807a4df7fd0"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 2, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("deb83b5e-0b6a-42d0-a5ee-60cf7cd2a613"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 4, "E", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("e09604b4-107f-49c4-8f79-c0bc2b598d4c"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 9, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("e22e3fb3-ed70-4e77-bb45-926065253403"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 3, "D", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("e34bc0e2-bede-4ba9-8d37-79dc98b423eb"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 4, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("e3c4fdb3-dbbd-4152-9b21-cfba66fff3b6"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 3, "D", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("e3ee3120-76c4-4625-9e0e-d986e3231de7"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 8, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("e489c7bd-0fcf-4a54-be44-abd097a2883a"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 6, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("e534d642-749b-4481-8d7d-011edf5042fd"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 1, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("e55d725b-7a51-429b-98d3-5d3171b63121"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 7, "H", 3, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("e6adbb50-1e24-4eb9-b48c-77670aefd7e4"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 1, "B", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("e898d7ad-c3f3-4859-afed-b7a3502f9e49"), 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 3, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("ea73ece5-9d7f-4fde-9251-35e05a8b6f40"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 2, "C", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("eb83d350-95e7-46b5-8a1a-7d5dba675ff3"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 2, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("ebadba1b-ef5d-460f-9c42-b5b1d17c6ab5"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 5, "F", 7, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("f429246e-b49e-4c52-a1b3-1e05d8393a87"), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 5, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("f6909dad-8ac8-4a19-88a3-14cdd1564053"), 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 4, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("f7b1423b-abe5-47ff-8e2e-c28bf5ec6a64"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 10, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("f90e4c5e-ebc8-4e03-86a2-63b2f55bae85"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 5, "F", 10, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("f92e4355-e411-4f3a-a1af-9ea165646145"), 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 2, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("f985d863-5b76-4abf-bb4f-1079b68d42cd"), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 0, "A", 8, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("f9b46d1a-e28d-4bad-ac05-9c0140275832"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 7, "H", 9, new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("fa4f5093-6847-4765-bcd0-756d7a69ffe4"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("fb0437f8-1410-438a-8aa8-99d0560483f1"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 5, "F", 10, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("fb47d0a2-d011-4e96-9d9e-1fee2eac64d5"), 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000002"), 4, "E", 10, new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("fc94bda4-0008-4312-8730-b6b3cfd1b18c"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 1, "B", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("fd4883ef-a50e-4fec-9d8d-ef74bd0375d2"), 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 0, "A", 9, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("fe0fdc82-195f-48a5-9778-ad2a52a9bf1c"), 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000004"), 4, "E", 7, new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("ff0247d0-5d4b-4f67-87c8-bc11375b16d3"), 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-0000-0000-0000-000000000001"), 6, "G", 6, new Guid("55555555-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "Showing",
                columns: new[] { "Id", "DateCreate", "DateUpdate", "IsActive", "MaxTicketsPerUser", "MovieId", "Price", "RoomId", "TimeStart" },
                values: new object[,]
                {
                    { new Guid("66666666-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, new Guid("22222222-0000-0000-0000-000000000001"), 85000m, new Guid("44444444-0000-0000-0000-000000000001"), new DateTime(2025, 6, 1, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, new Guid("22222222-0000-0000-0000-000000000001"), 85000m, new Guid("44444444-0000-0000-0000-000000000001"), new DateTime(2025, 6, 1, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, new Guid("22222222-0000-0000-0000-000000000002"), 90000m, new Guid("44444444-0000-0000-0000-000000000001"), new DateTime(2025, 6, 1, 17, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, new Guid("22222222-0000-0000-0000-000000000003"), 75000m, new Guid("44444444-0000-0000-0000-000000000002"), new DateTime(2025, 6, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, new Guid("22222222-0000-0000-0000-000000000004"), 100000m, new Guid("44444444-0000-0000-0000-000000000002"), new DateTime(2025, 6, 1, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 10, new Guid("22222222-0000-0000-0000-000000000004"), 120000m, new Guid("44444444-0000-0000-0000-000000000003"), new DateTime(2025, 6, 1, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 10, new Guid("22222222-0000-0000-0000-000000000007"), 120000m, new Guid("44444444-0000-0000-0000-000000000003"), new DateTime(2025, 6, 1, 15, 30, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, new Guid("22222222-0000-0000-0000-000000000005"), 80000m, new Guid("44444444-0000-0000-0000-000000000004"), new DateTime(2025, 6, 2, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, new Guid("22222222-0000-0000-0000-000000000006"), 80000m, new Guid("44444444-0000-0000-0000-000000000004"), new DateTime(2025, 6, 2, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, new Guid("22222222-0000-0000-0000-000000000007"), 90000m, new Guid("44444444-0000-0000-0000-000000000006"), new DateTime(2025, 6, 2, 19, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_AppUserId",
                table: "Booking",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ShowingId",
                table: "Booking",
                column: "ShowingId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_CategoryId",
                table: "Movie",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Title",
                table: "Movie",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_TheaterId",
                table: "Room",
                column: "TheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_RoomId",
                table: "Seat",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_SeatTypeId",
                table: "Seat",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Showing_MovieId",
                table: "Showing",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Showing_RoomId",
                table: "Showing",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_BookingId",
                table: "Ticket",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_SeatId",
                table: "Ticket",
                column: "SeatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Showing");

            migrationBuilder.DropTable(
                name: "SeatType");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Theater");
        }
    }
}
