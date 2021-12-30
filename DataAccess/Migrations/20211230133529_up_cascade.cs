using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Bangs_DS_Trinhs_ID_Trinh",
                table: "DS_Bangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Diems_DS_Caps_ID_Cap",
                table: "DS_Diems");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Diems_DS_Vongs_ID_Vong",
                table: "DS_Diems");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Vongs_ID_Vong",
                table: "DS_Trans");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trinhs_DS_Giais_ID_Giai",
                table: "DS_Trinhs");

            migrationBuilder.AddColumn<string>(
                name: "Ma_Cap",
                table: "DS_VDVs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Giai",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Vong",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Vong",
                table: "DS_Diems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Cap",
                table: "DS_Diems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Diems",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

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
                table: "DS_Caps",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Trinh",
                table: "DS_Bangs",
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
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Bangs_DS_Trinhs_ID_Trinh",
                table: "DS_Bangs",
                column: "ID_Trinh",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps",
                column: "ID_Bang",
                principalTable: "DS_Bangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Diems_DS_Caps_ID_Cap",
                table: "DS_Diems",
                column: "ID_Cap",
                principalTable: "DS_Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Diems_DS_Vongs_ID_Vong",
                table: "DS_Diems",
                column: "ID_Vong",
                principalTable: "DS_Vongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Vongs_ID_Vong",
                table: "DS_Trans",
                column: "ID_Vong",
                principalTable: "DS_Vongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trinhs_DS_Giais_ID_Giai",
                table: "DS_Trinhs",
                column: "ID_Giai",
                principalTable: "DS_Giais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Bangs_DS_Trinhs_ID_Trinh",
                table: "DS_Bangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Diems_DS_Caps_ID_Cap",
                table: "DS_Diems");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Diems_DS_Vongs_ID_Vong",
                table: "DS_Diems");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Vongs_ID_Vong",
                table: "DS_Trans");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trinhs_DS_Giais_ID_Giai",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "Ma_Cap",
                table: "DS_VDVs");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Giai",
                table: "DS_Trinhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Vong",
                table: "DS_Trans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Vong",
                table: "DS_Diems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Cap",
                table: "DS_Diems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Diems",
                type: "decimal(5,3)",
                nullable: true,
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
                table: "DS_Caps",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Trinh",
                table: "DS_Bangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Bangs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Bangs_DS_Trinhs_ID_Trinh",
                table: "DS_Bangs",
                column: "ID_Trinh",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                table: "DS_Caps",
                column: "ID_Bang",
                principalTable: "DS_Bangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Diems_DS_Caps_ID_Cap",
                table: "DS_Diems",
                column: "ID_Cap",
                principalTable: "DS_Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Diems_DS_Vongs_ID_Vong",
                table: "DS_Diems",
                column: "ID_Vong",
                principalTable: "DS_Vongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Vongs_ID_Vong",
                table: "DS_Trans",
                column: "ID_Vong",
                principalTable: "DS_Vongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trinhs_DS_Giais_ID_Giai",
                table: "DS_Trinhs",
                column: "ID_Giai",
                principalTable: "DS_Giais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
