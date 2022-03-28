using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_dsVDVDiem_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_VDVDiem_DS_VDVs_ID_Vdv",
                table: "DS_VDVDiem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DS_VDVDiem",
                table: "DS_VDVDiem");

            migrationBuilder.RenameTable(
                name: "DS_VDVDiem",
                newName: "DS_VDVDiems");

            migrationBuilder.RenameIndex(
                name: "IX_DS_VDVDiem_ID_Vdv",
                table: "DS_VDVDiems",
                newName: "IX_DS_VDVDiems_ID_Vdv");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DS_VDVDiems",
                table: "DS_VDVDiems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_VDVDiems_DS_VDVs_ID_Vdv",
                table: "DS_VDVDiems",
                column: "ID_Vdv",
                principalTable: "DS_VDVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_VDVDiems_DS_VDVs_ID_Vdv",
                table: "DS_VDVDiems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DS_VDVDiems",
                table: "DS_VDVDiems");

            migrationBuilder.RenameTable(
                name: "DS_VDVDiems",
                newName: "DS_VDVDiem");

            migrationBuilder.RenameIndex(
                name: "IX_DS_VDVDiems_ID_Vdv",
                table: "DS_VDVDiem",
                newName: "IX_DS_VDVDiem_ID_Vdv");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DS_VDVDiem",
                table: "DS_VDVDiem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_VDVDiem_DS_VDVs_ID_Vdv",
                table: "DS_VDVDiem",
                column: "ID_Vdv",
                principalTable: "DS_VDVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
