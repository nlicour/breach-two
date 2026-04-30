using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Breach.ApiOne.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weatherforecast",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TemperatureC = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ww",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    collation_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            // Insert sample data into weatherforecast table
            migrationBuilder.InsertData(
                table: "weatherforecast",
                columns: new[] { "Date", "TemperatureC", "Summary" },
                values: new object[,]
                {
                    { new DateOnly(2026, 4, 29), 20, "Sunny" },
                    { new DateOnly(2026, 4, 30), 18, "Cloudy" },
                    { new DateOnly(2026, 5, 1), 22, "Clear" },
                    { new DateOnly(2026, 5, 2), 15, "Rainy" }
                });

            // Insert sample data into ww table
            migrationBuilder.InsertData(
                table: "ww",
                columns: new[] { "name", "collation_name" },
                values: new object[,]
                {
                    { "User1", "Latin1_General_CI_AS" },
                    { "User2", "UTF-8" },
                    { "Admin", "Latin1_General_CI_AS" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weatherforecast");

            migrationBuilder.DropTable(
                name: "ww");
        }
    }
}
