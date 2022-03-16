using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_thong_bao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_Giai",
                table: "Thong_Baos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thong_Baos_ID_Giai",
                table: "Thong_Baos",
                column: "ID_Giai");

            migrationBuilder.AddForeignKey(
                name: "FK_Thong_Baos_DS_Giais_ID_Giai",
                table: "Thong_Baos",
                column: "ID_Giai",
                principalTable: "DS_Giais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thong_Baos_DS_Giais_ID_Giai",
                table: "Thong_Baos");

            migrationBuilder.DropIndex(
                name: "IX_Thong_Baos_ID_Giai",
                table: "Thong_Baos");

            migrationBuilder.DropColumn(
                name: "ID_Giai",
                table: "Thong_Baos");
        }
    }
}
