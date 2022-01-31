using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class down_ds_vdv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ma_Cap",
                table: "DS_VDVs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ma_Cap",
                table: "DS_VDVs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
