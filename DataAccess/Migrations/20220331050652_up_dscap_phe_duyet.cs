using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_dscap_phe_duyet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Phe_Duyet",
                table: "DS_Caps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Xac_Nhan",
                table: "DS_Caps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phe_Duyet",
                table: "DS_Caps");

            migrationBuilder.DropColumn(
                name: "Xac_Nhan",
                table: "DS_Caps");
        }
    }
}
