using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_allTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_Trinhs_ID_Trinh",
                table: "DS_Caps");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_VDVs_ID_Vdv1",
                table: "DS_Caps");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Diems_DS_Trinhs_ID_Trinh",
                table: "DS_Diems");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans");

            migrationBuilder.DropIndex(
                name: "IX_DS_Diems_ID_Trinh",
                table: "DS_Diems");

            migrationBuilder.DropColumn(
                name: "ID_Trinh",
                table: "DS_Diems");

            migrationBuilder.RenameColumn(
                name: "GhiChu",
                table: "Users",
                newName: "Ghi_Chu");

            migrationBuilder.RenameColumn(
                name: "HienThi",
                table: "Thong_Baos",
                newName: "Hien_Thi");

            migrationBuilder.RenameColumn(
                name: "FileTB",
                table: "Thong_Baos",
                newName: "File_TB");

            migrationBuilder.RenameColumn(
                name: "MaKhuVuc",
                table: "Khu_Vucs",
                newName: "Ma_KhuVuc");

            migrationBuilder.RenameColumn(
                name: "MaVong",
                table: "DS_Vongs",
                newName: "Ma_Vong");

            migrationBuilder.RenameColumn(
                name: "KhachMoi",
                table: "DS_VDVs",
                newName: "Khach_Moi");

            migrationBuilder.RenameColumn(
                name: "FileAnh",
                table: "DS_VDVs",
                newName: "File_Anh");

            migrationBuilder.RenameColumn(
                name: "DiemCu",
                table: "DS_VDVs",
                newName: "Diem_Cu");

            migrationBuilder.RenameColumn(
                name: "CongTy",
                table: "DS_VDVs",
                newName: "Cong_Ty");

            migrationBuilder.RenameColumn(
                name: "ChucVu",
                table: "DS_VDVs",
                newName: "Chuc_Vu");

            migrationBuilder.RenameColumn(
                name: "Id_Giai",
                table: "DS_VDVDiem",
                newName: "ID_Giai");

            migrationBuilder.RenameColumn(
                name: "TongDiem",
                table: "DS_Trinhs",
                newName: "Tong_Diem");

            migrationBuilder.RenameColumn(
                name: "DiemTru",
                table: "DS_Trinhs",
                newName: "Diem_Tru");

            migrationBuilder.RenameColumn(
                name: "ChenhLech",
                table: "DS_Trinhs",
                newName: "Chenh_Lech");

            migrationBuilder.RenameColumn(
                name: "GiaiMoi",
                table: "DS_Giais",
                newName: "Giai_Moi");

            migrationBuilder.RenameColumn(
                name: "GhiChu",
                table: "DS_Giais",
                newName: "Ghi_Chu");

            migrationBuilder.RenameColumn(
                name: "MaCap",
                table: "DS_Caps",
                newName: "Ma_Cap");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ngay",
                table: "Thong_Baos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Cap1",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Vdv1",
                table: "DS_Caps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Trinh",
                table: "DS_Caps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Caps",
                type: "decimal(6,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_Trinhs_ID_Trinh",
                table: "DS_Caps",
                column: "ID_Trinh",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_VDVs_ID_Vdv1",
                table: "DS_Caps",
                column: "ID_Vdv1",
                principalTable: "DS_VDVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans",
                column: "ID_Cap1",
                principalTable: "DS_Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_Trinhs_ID_Trinh",
                table: "DS_Caps");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_VDVs_ID_Vdv1",
                table: "DS_Caps");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans");

            migrationBuilder.RenameColumn(
                name: "Ghi_Chu",
                table: "Users",
                newName: "GhiChu");

            migrationBuilder.RenameColumn(
                name: "Hien_Thi",
                table: "Thong_Baos",
                newName: "HienThi");

            migrationBuilder.RenameColumn(
                name: "File_TB",
                table: "Thong_Baos",
                newName: "FileTB");

            migrationBuilder.RenameColumn(
                name: "Ma_KhuVuc",
                table: "Khu_Vucs",
                newName: "MaKhuVuc");

            migrationBuilder.RenameColumn(
                name: "Ma_Vong",
                table: "DS_Vongs",
                newName: "MaVong");

            migrationBuilder.RenameColumn(
                name: "Khach_Moi",
                table: "DS_VDVs",
                newName: "KhachMoi");

            migrationBuilder.RenameColumn(
                name: "File_Anh",
                table: "DS_VDVs",
                newName: "FileAnh");

            migrationBuilder.RenameColumn(
                name: "Diem_Cu",
                table: "DS_VDVs",
                newName: "DiemCu");

            migrationBuilder.RenameColumn(
                name: "Cong_Ty",
                table: "DS_VDVs",
                newName: "CongTy");

            migrationBuilder.RenameColumn(
                name: "Chuc_Vu",
                table: "DS_VDVs",
                newName: "ChucVu");

            migrationBuilder.RenameColumn(
                name: "ID_Giai",
                table: "DS_VDVDiem",
                newName: "Id_Giai");

            migrationBuilder.RenameColumn(
                name: "Tong_Diem",
                table: "DS_Trinhs",
                newName: "TongDiem");

            migrationBuilder.RenameColumn(
                name: "Diem_Tru",
                table: "DS_Trinhs",
                newName: "DiemTru");

            migrationBuilder.RenameColumn(
                name: "Chenh_Lech",
                table: "DS_Trinhs",
                newName: "ChenhLech");

            migrationBuilder.RenameColumn(
                name: "Giai_Moi",
                table: "DS_Giais",
                newName: "GiaiMoi");

            migrationBuilder.RenameColumn(
                name: "Ghi_Chu",
                table: "DS_Giais",
                newName: "GhiChu");

            migrationBuilder.RenameColumn(
                name: "Ma_Cap",
                table: "DS_Caps",
                newName: "MaCap");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ngay",
                table: "Thong_Baos",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Cap1",
                table: "DS_Trans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ID_Trinh",
                table: "DS_Diems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Vdv1",
                table: "DS_Caps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Trinh",
                table: "DS_Caps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Caps",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Diems_ID_Trinh",
                table: "DS_Diems",
                column: "ID_Trinh");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_Trinhs_ID_Trinh",
                table: "DS_Caps",
                column: "ID_Trinh",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_VDVs_ID_Vdv1",
                table: "DS_Caps",
                column: "ID_Vdv1",
                principalTable: "DS_VDVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Diems_DS_Trinhs_ID_Trinh",
                table: "DS_Diems",
                column: "ID_Trinh",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans",
                column: "ID_Cap1",
                principalTable: "DS_Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
