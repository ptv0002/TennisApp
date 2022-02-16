using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_ho_ten_ds_vdv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ho",
                table: "DS_VDVs");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "DS_VDVs");

            migrationBuilder.AlterColumn<string>(
                name: "Ten_Tat",
                table: "DS_VDVs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ho_Ten",
                table: "DS_VDVs",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "DS_Bangs",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ho_Ten",
                table: "DS_VDVs");

            migrationBuilder.AlterColumn<string>(
                name: "Ten_Tat",
                table: "DS_VDVs",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ho",
                table: "DS_VDVs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "DS_VDVs",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "DS_Bangs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)");
        }
    }
}
