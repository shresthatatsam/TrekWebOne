using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoles.Migrations
{
    /// <inheritdoc />
    public partial class TrekPackageFixedPricings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrekPackageFixedPricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinPeople = table.Column<int>(type: "int", nullable: false),
                    MaxPeople = table.Column<int>(type: "int", nullable: false),
                    PricePerPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrekPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrekPackageFixedPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrekPackageFixedPricings_TrekPackages_TrekPackageId",
                        column: x => x.TrekPackageId,
                        principalTable: "TrekPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrekPackageFixedPricings_TrekPackageId",
                table: "TrekPackageFixedPricings",
                column: "TrekPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrekPackageFixedPricings");
        }
    }
}
