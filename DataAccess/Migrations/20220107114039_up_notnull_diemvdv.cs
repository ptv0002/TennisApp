using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_notnull_diemvdv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps");

            migrationBuilder.AddColumn<bool>(
                name: "HienThi",
                table: "Thong_Baos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Tham_Gia",
                table: "DS_VDVs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "DS_VDVs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "KhachMoi",
                table: "DS_VDVs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiemCu",
                table: "DS_VDVs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Diem",
                table: "DS_VDVs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Trinh",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TongDiem",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Diem_PB",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiemTru",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChenhLech",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Kq_2",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Kq_1",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ngay",
                table: "DS_Giais",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Diems",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Bang",
                table: "DS_Caps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Bangs",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DS_VDVDiem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Giai = table.Column<int>(type: "int", nullable: false),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: false),
                    ID_Vdv = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_VDVDiem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_VDVDiem_DS_VDVs_ID_Vdv",
                        column: x => x.ID_Vdv,
                        principalTable: "DS_VDVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DS_VDVDiem_ID_Vdv",
                table: "DS_VDVDiem",
                column: "ID_Vdv");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps",
                column: "ID_Bang",
                principalTable: "DS_Bangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps");

            migrationBuilder.DropTable(
                name: "DS_VDVDiem");

            migrationBuilder.DropColumn(
                name: "HienThi",
                table: "Thong_Baos");

            migrationBuilder.AlterColumn<bool>(
                name: "Tham_Gia",
                table: "DS_VDVs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "DS_VDVs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "KhachMoi",
                table: "DS_VDVs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DiemCu",
                table: "DS_VDVs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Diem",
                table: "DS_VDVs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Trinh",
                table: "DS_Trinhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TongDiem",
                table: "DS_Trinhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<int>(
                name: "Diem_PB",
                table: "DS_Trinhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DiemTru",
                table: "DS_Trinhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChenhLech",
                table: "DS_Trinhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Kq_2",
                table: "DS_Trans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Kq_1",
                table: "DS_Trans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ngay",
                table: "DS_Giais",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Diems",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Bang",
                table: "DS_Caps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Bangs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps",
                column: "ID_Bang",
                principalTable: "DS_Bangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
