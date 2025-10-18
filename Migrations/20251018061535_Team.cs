using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoles.Migrations
{
    /// <inheritdoc />
    public partial class Team : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "TeamMembers",
                newName: "TwitterUrl");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "TeamMembers",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TeamMembers",
                newName: "LinkedInUrl");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "TeamMembers",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TeamMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "TeamMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "TeamMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "TwitterUrl",
                table: "TeamMembers",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "TeamMembers",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "LinkedInUrl",
                table: "TeamMembers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "TeamMembers",
                newName: "Bio");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TeamMembers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
