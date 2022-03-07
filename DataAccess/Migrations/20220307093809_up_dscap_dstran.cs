using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_dscap_dstran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chon_Cap",
                table: "DS_Trans");

            migrationBuilder.AddColumn<string>(
                name: "Chon_Cap_1",
                table: "DS_Trans",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chon_Cap_2",
                table: "DS_Trans",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Hieu_so",
                table: "DS_Caps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chon_Cap_1",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "Chon_Cap_2",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "Hieu_so",
                table: "DS_Caps");

            migrationBuilder.AddColumn<string>(
                name: "Chon_Cap",
                table: "DS_Trans",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
