using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AudioGearTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewScore = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "France", "Focal" },
                    { 2, "Japan", "Sony" },
                    { 3, "USA", "Benchmark" }
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "BrandId", "ModelName", "Notes", "Price", "PurchaseDate", "ReviewScore", "Type" },
                values: new object[,]
                {
                    { 1, 1, "Utopia", "大烏，主力耳機", 130000m, null, 9.8000000000000007, 0 },
                    { 2, 1, "Clear MG", "偏暖厚、密度高", 45000m, null, 9.0, 0 },
                    { 3, 2, "IER-M9", "監聽入耳式", 28000m, null, 8.5, 0 },
                    { 4, 3, "DAC3 HGC", "借用中", 60000m, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 8.8000000000000007, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_BrandId",
                table: "Equipments",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
