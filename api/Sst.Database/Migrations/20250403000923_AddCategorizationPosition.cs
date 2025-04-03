using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sst.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCategorizationPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Categorizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Categorizations");
        }
    }
}
