using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sst.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvailableBalance",
                table: "Accounts",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBalance",
                table: "Accounts",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBalance",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                table: "Accounts");
        }
    }
}
