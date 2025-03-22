using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChangeDataCapture.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ratting = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Genre", "Ratting", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("0195bda1-76dc-735c-9361-cc0d0b311d7f"), 2, 0m, new DateOnly(1994, 9, 23), "The Shawshank Redemption" },
                    { new Guid("0195bda1-76dc-76a2-a0c0-7159220ec134"), 4, 0m, new DateOnly(1999, 3, 31), "The Matrix" },
                    { new Guid("0195bda1-76dc-7f46-89a1-75105b2d30d2"), 4, 0m, new DateOnly(2003, 5, 15), "The Matrix Reloaded" },
                    { new Guid("0195bda1-76dc-7fdc-b9b3-b882c033b492"), 4, 0m, new DateOnly(2003, 11, 5), "The Matrix Revolutions" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
