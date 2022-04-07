using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_ds_trinh_bienDo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Tin_Nong",
                table: "Thong_Baos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BD_Duoi",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BD_Tren",
                table: "DS_Trinhs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tin_Nong",
                table: "Thong_Baos");

            migrationBuilder.DropColumn(
                name: "BD_Duoi",
                table: "DS_Trinhs");

            migrationBuilder.DropColumn(
                name: "BD_Tren",
                table: "DS_Trinhs");
        }
    }
}
