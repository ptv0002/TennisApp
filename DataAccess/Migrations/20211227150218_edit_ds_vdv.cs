using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_ds_vdv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ten_Tat",
                table: "DS_VDVs",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Tham_Gia",
                table: "DS_VDVs",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tham_Gia",
                table: "DS_VDVs");

            migrationBuilder.AlterColumn<string>(
                name: "Ten_Tat",
                table: "DS_VDVs",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_VoDich",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_TuKet",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_ChungKet",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_Bang",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TL_BanKet",
                table: "DS_Trinhs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldNullable: true);
        }
    }
}
