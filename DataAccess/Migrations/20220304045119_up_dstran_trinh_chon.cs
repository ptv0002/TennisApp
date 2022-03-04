using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_dstran_trinh_chon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Chon_Cap",
                table: "DS_Trans",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DS_TrinhId",
                table: "DS_Trans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID_Trinh",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_DS_TrinhId",
                table: "DS_Trans",
                column: "DS_TrinhId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Trinhs_DS_TrinhId",
                table: "DS_Trans",
                column: "DS_TrinhId",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Trinhs_DS_TrinhId",
                table: "DS_Trans");

            migrationBuilder.DropIndex(
                name: "IX_DS_Trans_DS_TrinhId",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "Chon_Cap",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "DS_TrinhId",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "ID_Trinh",
                table: "DS_Trans");
        }
    }
}
