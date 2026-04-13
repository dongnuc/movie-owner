using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_StructureCode.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IsUsedRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "RefreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "RefreshToken");
        }
    }
}
