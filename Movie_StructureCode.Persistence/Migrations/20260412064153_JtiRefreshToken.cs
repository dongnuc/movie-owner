using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movie_StructureCode.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class JtiRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                    name: "JwtId",
                    table: "RefreshToken",
                    type: "nvarchar(max)",
                    nullable: false,
                    defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JwtId",
                table: "RefreshToken");
        }
    }
}
