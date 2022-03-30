using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DS_Giais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Giai_Moi = table.Column<bool>(type: "bit", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Giais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Khu_Vucs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_KhuVuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khu_Vucs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DS_Trinhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Trinh = table.Column<int>(type: "int", nullable: false),
                    TL_VoDich = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TL_ChungKet = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TL_BanKet = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TL_TuKet = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TL_Bang = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    Diem_PB = table.Column<int>(type: "int", nullable: false),
                    Tong_Diem = table.Column<int>(type: "int", nullable: false),
                    Diem_Tru = table.Column<int>(type: "int", nullable: false),
                    Chenh_Lech = table.Column<int>(type: "int", nullable: false),
                    ID_Giai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Trinhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_Trinhs_DS_Giais_ID_Giai",
                        column: x => x.ID_Giai,
                        principalTable: "DS_Giais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thong_Baos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    File_Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hien_Thi = table.Column<bool>(type: "bit", nullable: false),
                    ID_Giai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thong_Baos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thong_Baos_DS_Giais_ID_Giai",
                        column: x => x.ID_Giai,
                        principalTable: "DS_Giais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DS_VDVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ho_Ten = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Ten_Tat = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Phe_Duyet = table.Column<bool>(type: "bit", nullable: true),
                    Tham_Gia = table.Column<bool>(type: "bit", nullable: false),
                    Gioi_Tinh = table.Column<bool>(type: "bit", nullable: false),
                    CLB = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Khach_Moi = table.Column<bool>(type: "bit", nullable: false),
                    File_Anh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: false),
                    Diem_Cu = table.Column<int>(type: "int", nullable: false),
                    Cong_Ty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chuc_Vu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_KhuVuc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_VDVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_VDVs_Khu_Vucs_ID_KhuVuc",
                        column: x => x.ID_KhuVuc,
                        principalTable: "Khu_Vucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DS_Bangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diem = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    ID_Trinh = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Bangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_Bangs_DS_Trinhs_ID_Trinh",
                        column: x => x.ID_Trinh,
                        principalTable: "DS_Trinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DS_VDVDiems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Trinh = table.Column<int>(type: "int", nullable: true),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: false),
                    ID_Vdv = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_VDVDiems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_VDVDiems_DS_VDVs_ID_Vdv",
                        column: x => x.ID_Vdv,
                        principalTable: "DS_VDVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DS_Caps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_Cap = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Diem = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    Tran_Thang = table.Column<int>(type: "int", nullable: false),
                    Boc_Tham = table.Column<int>(type: "int", nullable: false),
                    Xep_Hang = table.Column<int>(type: "int", nullable: false),
                    ID_Trinh = table.Column<int>(type: "int", nullable: false),
                    ID_Bang = table.Column<int>(type: "int", nullable: true),
                    ID_Vdv1 = table.Column<int>(type: "int", nullable: false),
                    ID_Vdv2 = table.Column<int>(type: "int", nullable: true),
                    Hieu_so = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Caps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_Caps_DS_Bangs_ID_Bang",
                        column: x => x.ID_Bang,
                        principalTable: "DS_Bangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DS_Caps_DS_Trinhs_ID_Trinh",
                        column: x => x.ID_Trinh,
                        principalTable: "DS_Trinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DS_Caps_DS_VDVs_ID_Vdv1",
                        column: x => x.ID_Vdv1,
                        principalTable: "DS_VDVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DS_Caps_DS_VDVs_ID_Vdv2",
                        column: x => x.ID_Vdv2,
                        principalTable: "DS_VDVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DS_Diems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diem = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    ID_Cap = table.Column<int>(type: "int", nullable: false),
                    ID_Vong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Diems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_Diems_DS_Caps_ID_Cap",
                        column: x => x.ID_Cap,
                        principalTable: "DS_Caps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DS_Trans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_Tran = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Kq_1 = table.Column<int>(type: "int", nullable: false),
                    Kq_2 = table.Column<int>(type: "int", nullable: false),
                    Chon_Cap_1 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Chon_Cap_2 = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ID_Cap1 = table.Column<int>(type: "int", nullable: true),
                    ID_Cap2 = table.Column<int>(type: "int", nullable: true),
                    Ma_Vong = table.Column<int>(type: "int", nullable: false),
                    ID_Trinh = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Trans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DS_Trans_DS_Caps_ID_Cap1",
                        column: x => x.ID_Cap1,
                        principalTable: "DS_Caps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DS_Trans_DS_Caps_ID_Cap2",
                        column: x => x.ID_Cap2,
                        principalTable: "DS_Caps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DS_Trans_DS_Trinhs_ID_Trinh",
                        column: x => x.ID_Trinh,
                        principalTable: "DS_Trinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DS_Bangs_ID_Trinh",
                table: "DS_Bangs",
                column: "ID_Trinh");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Caps_ID_Bang",
                table: "DS_Caps",
                column: "ID_Bang");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Caps_ID_Trinh",
                table: "DS_Caps",
                column: "ID_Trinh");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Caps_ID_Vdv1",
                table: "DS_Caps",
                column: "ID_Vdv1");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Caps_ID_Vdv2",
                table: "DS_Caps",
                column: "ID_Vdv2");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Diems_ID_Cap",
                table: "DS_Diems",
                column: "ID_Cap");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_ID_Cap1",
                table: "DS_Trans",
                column: "ID_Cap1");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_ID_Cap2",
                table: "DS_Trans",
                column: "ID_Cap2");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trans_ID_Trinh",
                table: "DS_Trans",
                column: "ID_Trinh");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Trinhs_ID_Giai",
                table: "DS_Trinhs",
                column: "ID_Giai");

            migrationBuilder.CreateIndex(
                name: "IX_DS_VDVDiems_ID_Vdv",
                table: "DS_VDVDiems",
                column: "ID_Vdv");

            migrationBuilder.CreateIndex(
                name: "IX_DS_VDVs_ID_KhuVuc",
                table: "DS_VDVs",
                column: "ID_KhuVuc");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Thong_Baos_ID_Giai",
                table: "Thong_Baos",
                column: "ID_Giai");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DS_Diems");

            migrationBuilder.DropTable(
                name: "DS_Trans");

            migrationBuilder.DropTable(
                name: "DS_VDVDiems");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Thong_Baos");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "DS_Caps");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DS_Bangs");

            migrationBuilder.DropTable(
                name: "DS_VDVs");

            migrationBuilder.DropTable(
                name: "DS_Trinhs");

            migrationBuilder.DropTable(
                name: "Khu_Vucs");

            migrationBuilder.DropTable(
                name: "DS_Giais");
        }
    }
}
