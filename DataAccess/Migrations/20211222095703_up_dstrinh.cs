using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_dstrinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DS_ThongBaos");

            migrationBuilder.AddColumn<int>(
                name: "Diem_PB",
                table: "DS_Trinhs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "DS_Giais",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Thong_Baos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileTB = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thong_Baos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Thong_Baos");

            migrationBuilder.DropColumn(
                name: "Diem_PB",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "TL_BanKet",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "TL_Bang",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "TL_ChungKet",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "TL_TuKet",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "TL_VoDich",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "DS_Giais");

            migrationBuilder.CreateTable(
                name: "DS_ThongBaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileTB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_ThongBaos", x => x.Id);
                });
        }
    }
}
