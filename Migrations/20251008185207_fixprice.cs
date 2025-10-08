using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoles.Migrations
{
    /// <inheritdoc />
    public partial class fixprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPeople",
                table: "TrekPackageFixedPricings");

            migrationBuilder.DropColumn(
                name: "MinPeople",
                table: "TrekPackageFixedPricings");

            migrationBuilder.AlterColumn<string>(
                name: "ImageType",
                table: "TrekPackageImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ImageType",
                table: "TrekPackageImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxPeople",
                table: "TrekPackageFixedPricings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinPeople",
                table: "TrekPackageFixedPricings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
