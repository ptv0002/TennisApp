using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_ds_tran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tran_Thang",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Xep_Hang",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tran_Thang",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "Xep_Hang",
                table: "DS_Trans");
        }
    }
}
