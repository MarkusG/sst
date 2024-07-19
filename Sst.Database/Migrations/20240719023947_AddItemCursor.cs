﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sst.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddItemCursor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NextCursor",
                table: "Items",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextCursor",
                table: "Items");
        }
    }
}
