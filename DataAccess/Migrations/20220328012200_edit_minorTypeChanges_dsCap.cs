using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_minorTypeChanges_dsCap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Diems",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Caps",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Bangs",
                type: "decimal(8,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Diems",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Caps",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "DS_Bangs",
                type: "decimal(6,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,3)");
        }
    }
}
