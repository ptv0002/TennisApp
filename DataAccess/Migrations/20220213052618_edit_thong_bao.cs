using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class edit_thong_bao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans");

            migrationBuilder.RenameColumn(
                name: "File_TB",
                table: "Thong_Baos",
                newName: "File_Text");

            migrationBuilder.AddColumn<string>(
                name: "File_Path",
                table: "Thong_Baos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Cap1",
                table: "DS_Trans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans",
                column: "ID_Cap1",
                principalTable: "DS_Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans");

            migrationBuilder.DropColumn(
                name: "File_Path",
                table: "Thong_Baos");

            migrationBuilder.RenameColumn(
                name: "File_Text",
                table: "Thong_Baos",
                newName: "File_TB");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Cap1",
                table: "DS_Trans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                table: "DS_Trans",
                column: "ID_Cap1",
                principalTable: "DS_Caps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
