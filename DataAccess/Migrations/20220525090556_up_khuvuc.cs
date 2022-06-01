using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class up_khuvuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_KhuVuc",
                table: "Thong_Baos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_KhuVuc",
                table: "DS_VDVs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Nam_Sinh",
                table: "DS_VDVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ID_KhuVuc",
                table: "DS_Giais",
                type: "int",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_Menu = table.Column<int>(type: "int", nullable: false),
                    Link_Hinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link_Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_Giai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_DS_Giais_ID_Giai",
                        column: x => x.ID_Giai,
                        principalTable: "DS_Giais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Thong_Baos_ID_KhuVuc",
                table: "Thong_Baos",
                column: "ID_KhuVuc");

            migrationBuilder.CreateIndex(
                name: "IX_DS_VDVs_ID_KhuVuc",
                table: "DS_VDVs",
                column: "ID_KhuVuc");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Giais_ID_KhuVuc",
                table: "DS_Giais",
                column: "ID_KhuVuc");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_ID_Giai",
                table: "Medias",
                column: "ID_Giai");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Giais_Khu_Vucs_ID_KhuVuc",
                table: "DS_Giais",
                column: "ID_KhuVuc",
                principalTable: "Khu_Vucs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_VDVs_Khu_Vucs_ID_KhuVuc",
                table: "DS_VDVs",
                column: "ID_KhuVuc",
                principalTable: "Khu_Vucs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Thong_Baos_Khu_Vucs_ID_KhuVuc",
                table: "Thong_Baos",
                column: "ID_KhuVuc",
                principalTable: "Khu_Vucs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Giais_Khu_Vucs_ID_KhuVuc",
                table: "DS_Giais");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_VDVs_Khu_Vucs_ID_KhuVuc",
                table: "DS_VDVs");

            migrationBuilder.DropForeignKey(
                name: "FK_Thong_Baos_Khu_Vucs_ID_KhuVuc",
                table: "Thong_Baos");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Thong_Baos_ID_KhuVuc",
                table: "Thong_Baos");

            migrationBuilder.DropIndex(
                name: "IX_DS_VDVs_ID_KhuVuc",
                table: "DS_VDVs");

            migrationBuilder.DropIndex(
                name: "IX_DS_Giais_ID_KhuVuc",
                table: "DS_Giais");

            migrationBuilder.DropColumn(
                name: "ID_KhuVuc",
                table: "Thong_Baos");

            migrationBuilder.DropColumn(
                name: "Nam_Sinh",
                table: "DS_VDVs");

            migrationBuilder.DropColumn(
                name: "ID_KhuVuc",
                table: "DS_Giais");

            migrationBuilder.AlterColumn<int>(
                name: "ID_KhuVuc",
                table: "DS_VDVs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
