using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_dsVdv_dsVDVDiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Trinh",
                table: "DS_Diems");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "DS_VDVs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Trinh",
                table: "DS_VDVDiem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "DS_VDVs");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Trinh",
                table: "DS_VDVDiem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID_Trinh",
                table: "DS_Diems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
