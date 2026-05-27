using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_StructureCode.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTheaterIdInShowing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Showing_MovieId",
                table: "Showing");

            migrationBuilder.AddColumn<Guid>(
                name: "TheaterId",
                table: "Showing",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "StatusSeat",
                table: "Seat",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Showing_MovieId_TheaterId_IsActive",
                table: "Showing",
                columns: new[] { "MovieId", "TheaterId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Showing_TheaterId",
                table: "Showing",
                column: "TheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Showing_TheaterId_IsActive_MovieId",
                table: "Showing",
                columns: new[] { "TheaterId", "IsActive", "MovieId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Showing_MovieId_TheaterId_IsActive",
                table: "Showing");

            migrationBuilder.DropIndex(
                name: "IX_Showing_TheaterId",
                table: "Showing");

            migrationBuilder.DropIndex(
                name: "IX_Showing_TheaterId_IsActive_MovieId",
                table: "Showing");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Showing");

            migrationBuilder.AlterColumn<int>(
                name: "StatusSeat",
                table: "Seat",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Showing_MovieId",
                table: "Showing",
                column: "MovieId");
        }
    }
}
