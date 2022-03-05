using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class down_ds_vong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Diems_DS_Vongs_ID_Vong",
                table: "DS_Diems");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Trinhs_DS_TrinhId",
                table: "DS_Trans");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Vongs_ID_Vong",
                table: "DS_Trans");

            migrationBuilder.DropTable(
                name: "DS_Vongs");

            migrationBuilder.DropIndex(
                name: "IX_DS_Trans_DS_TrinhId",
                table: "DS_Trans");

            migrationBuilder.DropIndex(
                name: "IX_DS_Trans_ID_Vong",
                table: "DS_Trans");

            migrationBuilder.DropIndex(
                name: "IX_DS_Diems_ID_Vong",
                table: "DS_Diems");

            migrationBuilder.DropColumn(
                name: "DS_TrinhId",
                table: "DS_Trans");

            migrationBuilder.RenameColumn(
                name: "ID_Vong",
                table: "DS_Trans",
                newName: "Ma_Vong");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_ID_Trinh",
                table: "DS_Trans",
                column: "ID_Trinh");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Trinhs_ID_Trinh",
                table: "DS_Trans",
                column: "ID_Trinh",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Trans_DS_Trinhs_ID_Trinh",
                table: "DS_Trans");

            migrationBuilder.DropIndex(
                name: "IX_DS_Trans_ID_Trinh",
                table: "DS_Trans");

            migrationBuilder.RenameColumn(
                name: "Ma_Vong",
                table: "DS_Trans",
                newName: "ID_Vong");

            migrationBuilder.AddColumn<int>(
                name: "DS_TrinhId",
                table: "DS_Trans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DS_Vongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_Vong = table.Column<int>(type: "int", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Vongs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_DS_TrinhId",
                table: "DS_Trans",
                column: "DS_TrinhId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_ID_Vong",
                table: "DS_Trans",
                column: "ID_Vong");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Diems_ID_Vong",
                table: "DS_Diems",
                column: "ID_Vong");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Diems_DS_Vongs_ID_Vong",
                table: "DS_Diems",
                column: "ID_Vong",
                principalTable: "DS_Vongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Trinhs_DS_TrinhId",
                table: "DS_Trans",
                column: "DS_TrinhId",
                principalTable: "DS_Trinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Trans_DS_Vongs_ID_Vong",
                table: "DS_Trans",
                column: "ID_Vong",
                principalTable: "DS_Vongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
