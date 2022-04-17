using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_multiple_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_Anh",
                table: "DS_VDVs");

            migrationBuilder.AddColumn<int>(
                name: "Max_Point",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Min_Point",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Ngay",
                table: "DS_Caps",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Max_Point",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "Min_Point",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "Ngay",
                table: "DS_Caps");

            migrationBuilder.AddColumn<string>(
                name: "File_Anh",
                table: "DS_VDVs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
