using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_dsdiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID_Giai",
                table: "DS_VDVDiem",
                newName: "ID_Trinh");

            migrationBuilder.AddColumn<int>(
                name: "ID_Trinh",
                table: "DS_Diems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Trinh",
                table: "DS_Diems");

            migrationBuilder.RenameColumn(
                name: "ID_Trinh",
                table: "DS_VDVDiem",
                newName: "ID_Giai");
        }
    }
}
