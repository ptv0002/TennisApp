﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(TennisContext))]
    partial class TennisContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Ghi_Chu")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.DS_Bang", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Diem")
                        .HasColumnType("decimal(8,3)");

                    b.Property<int>("ID_Trinh")
                        .HasColumnType("int");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("ID_Trinh");

                    b.ToTable("DS_Bangs");
                });

            modelBuilder.Entity("Models.DS_Cap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Boc_Tham")
                        .HasColumnType("int");

                    b.Property<decimal>("Diem")
                        .HasColumnType("decimal(8,3)");

                    b.Property<int>("Hieu_so")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Bang")
                        .HasColumnType("int");

                    b.Property<int>("ID_Trinh")
                        .HasColumnType("int");

                    b.Property<int>("ID_Vdv1")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Vdv2")
                        .HasColumnType("int");

                    b.Property<string>("Ma_Cap")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime?>("Ngay")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Phe_Duyet")
                        .HasColumnType("bit");

                    b.Property<int>("Tran_Thang")
                        .HasColumnType("int");

                    b.Property<bool>("Xac_Nhan")
                        .HasColumnType("bit");

                    b.Property<int>("Xep_Hang")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Bang");

                    b.HasIndex("ID_Trinh");

                    b.HasIndex("ID_Vdv1");

                    b.HasIndex("ID_Vdv2");

                    b.ToTable("DS_Caps");
                });

            modelBuilder.Entity("Models.DS_Diem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Diem")
                        .HasColumnType("decimal(8,3)");

                    b.Property<int>("ID_Cap")
                        .HasColumnType("int");

                    b.Property<int>("ID_Vong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Cap");

                    b.ToTable("DS_Diems");
                });

            modelBuilder.Entity("Models.DS_Giai", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ghi_Chu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Giai_Moi")
                        .HasColumnType("bit");

                    b.Property<int?>("ID_KhuVuc")
                        .HasColumnType("int");

                    b.Property<DateTime>("Ngay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ten")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.HasKey("Id");

                    b.HasIndex("ID_KhuVuc");

                    b.ToTable("DS_Giais");
                });

            modelBuilder.Entity("Models.DS_Tran", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Chon_Cap_1")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Chon_Cap_2")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int?>("ID_Cap1")
                        .HasColumnType("int");

                    b.Property<int?>("ID_Cap2")
                        .HasColumnType("int");

                    b.Property<int>("ID_Trinh")
                        .HasColumnType("int");

                    b.Property<int>("Kq_1")
                        .HasColumnType("int");

                    b.Property<int>("Kq_2")
                        .HasColumnType("int");

                    b.Property<string>("Ma_Tran")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("Ma_Vong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Cap1");

                    b.HasIndex("ID_Cap2");

                    b.HasIndex("ID_Trinh");

                    b.ToTable("DS_Trans");
                });

            modelBuilder.Entity("Models.DS_Trinh", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BD_Duoi")
                        .HasColumnType("int");

                    b.Property<int>("BD_Tren")
                        .HasColumnType("int");

                    b.Property<int>("Chenh_Lech")
                        .HasColumnType("int");

                    b.Property<int>("Diem_PB")
                        .HasColumnType("int");

                    b.Property<int>("Diem_Tru")
                        .HasColumnType("int");

                    b.Property<int>("ID_Giai")
                        .HasColumnType("int");

                    b.Property<int>("Max_Point")
                        .HasColumnType("int");

                    b.Property<int>("Min_Point")
                        .HasColumnType("int");

                    b.Property<decimal>("TL_BanKet")
                        .HasColumnType("decimal(8,3)");

                    b.Property<decimal>("TL_Bang")
                        .HasColumnType("decimal(8,3)");

                    b.Property<decimal>("TL_ChungKet")
                        .HasColumnType("decimal(8,3)");

                    b.Property<decimal>("TL_TuKet")
                        .HasColumnType("decimal(8,3)");

                    b.Property<decimal>("TL_VoDich")
                        .HasColumnType("decimal(8,3)");

                    b.Property<int>("Tong_Diem")
                        .HasColumnType("int");

                    b.Property<int>("Trinh")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Giai");

                    b.ToTable("DS_Trinhs");
                });

            modelBuilder.Entity("Models.DS_VDV", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CLB")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Chuc_Vu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cong_Ty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_PD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Diem")
                        .HasColumnType("int");

                    b.Property<int>("Diem_Cu")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("Gioi_Tinh")
                        .HasColumnType("bit");

                    b.Property<string>("Ho_Ten")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("ID_KhuVuc")
                        .HasColumnType("int");

                    b.Property<bool>("Khach_Moi")
                        .HasColumnType("bit");

                    b.Property<int>("Nam_Sinh")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool?>("Phe_Duyet")
                        .HasColumnType("bit");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten_Tat")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("Tham_Gia")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ID_KhuVuc");

                    b.ToTable("DS_VDVs");
                });

            modelBuilder.Entity("Models.DS_VDVDiem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Diem")
                        .HasColumnType("int");

                    b.Property<string>("Ghi_Chu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ID_Trinh")
                        .HasColumnType("int");

                    b.Property<int>("ID_Vdv")
                        .HasColumnType("int");

                    b.Property<DateTime>("Ngay")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ID_Vdv");

                    b.ToTable("DS_VDVDiems");
                });

            modelBuilder.Entity("Models.Khu_Vuc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ma_KhuVuc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Khu_Vucs");
                });

            modelBuilder.Entity("Models.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ID_Giai")
                        .HasColumnType("int");

                    b.Property<string>("Link_Hinh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link_Video")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ma_Menu")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Giai");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("Models.Thong_Bao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("File_Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File_Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Hien_Thi")
                        .HasColumnType("bit");

                    b.Property<int?>("ID_Giai")
                        .HasColumnType("int");

                    b.Property<int?>("ID_KhuVuc")
                        .HasColumnType("int");

                    b.Property<DateTime>("Ngay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ten")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<bool>("Tin_Nong")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ID_Giai");

                    b.HasIndex("ID_KhuVuc");

                    b.ToTable("Thong_Baos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.DS_Bang", b =>
                {
                    b.HasOne("Models.DS_Trinh", "DS_Trinh")
                        .WithMany("DS_Bang")
                        .HasForeignKey("ID_Trinh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DS_Trinh");
                });

            modelBuilder.Entity("Models.DS_Cap", b =>
                {
                    b.HasOne("Models.DS_Bang", "DS_Bang")
                        .WithMany("DS_Cap")
                        .HasForeignKey("ID_Bang");

                    b.HasOne("Models.DS_Trinh", "DS_Trinh")
                        .WithMany("DS_Cap")
                        .HasForeignKey("ID_Trinh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DS_VDV", "VDV1")
                        .WithMany()
                        .HasForeignKey("ID_Vdv1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DS_VDV", "VDV2")
                        .WithMany("DS_Caps")
                        .HasForeignKey("ID_Vdv2");

                    b.Navigation("DS_Bang");

                    b.Navigation("DS_Trinh");

                    b.Navigation("VDV1");

                    b.Navigation("VDV2");
                });

            modelBuilder.Entity("Models.DS_Diem", b =>
                {
                    b.HasOne("Models.DS_Cap", "DS_Cap")
                        .WithMany()
                        .HasForeignKey("ID_Cap")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DS_Cap");
                });

            modelBuilder.Entity("Models.DS_Giai", b =>
                {
                    b.HasOne("Models.Khu_Vuc", "Khu_Vucs")
                        .WithMany("DS_Giai")
                        .HasForeignKey("ID_KhuVuc");

                    b.Navigation("Khu_Vucs");
                });

            modelBuilder.Entity("Models.DS_Tran", b =>
                {
                    b.HasOne("Models.DS_Cap", "DS_Cap1")
                        .WithMany()
                        .HasForeignKey("ID_Cap1");

                    b.HasOne("Models.DS_Cap", "DS_Cap2")
                        .WithMany("DS_Trans")
                        .HasForeignKey("ID_Cap2");

                    b.HasOne("Models.DS_Trinh", "DS_Trinh")
                        .WithMany("DS_Tran")
                        .HasForeignKey("ID_Trinh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DS_Cap1");

                    b.Navigation("DS_Cap2");

                    b.Navigation("DS_Trinh");
                });

            modelBuilder.Entity("Models.DS_Trinh", b =>
                {
                    b.HasOne("Models.DS_Giai", "DS_Giai")
                        .WithMany("DS_Trinh")
                        .HasForeignKey("ID_Giai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DS_Giai");
                });

            modelBuilder.Entity("Models.DS_VDV", b =>
                {
                    b.HasOne("Models.Khu_Vuc", "Khu_Vucs")
                        .WithMany()
                        .HasForeignKey("ID_KhuVuc");

                    b.Navigation("Khu_Vucs");
                });

            modelBuilder.Entity("Models.DS_VDVDiem", b =>
                {
                    b.HasOne("Models.DS_VDV", "DS_VDV")
                        .WithMany("DS_VDV_Diem")
                        .HasForeignKey("ID_Vdv")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DS_VDV");
                });

            modelBuilder.Entity("Models.Media", b =>
                {
                    b.HasOne("Models.DS_Giai", "DS_Giai")
                        .WithMany()
                        .HasForeignKey("ID_Giai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DS_Giai");
                });

            modelBuilder.Entity("Models.Thong_Bao", b =>
                {
                    b.HasOne("Models.DS_Giai", "DS_Giai")
                        .WithMany()
                        .HasForeignKey("ID_Giai");

                    b.HasOne("Models.Khu_Vuc", "Khu_Vucs")
                        .WithMany("Thong_Baos")
                        .HasForeignKey("ID_KhuVuc");

                    b.Navigation("DS_Giai");

                    b.Navigation("Khu_Vucs");
                });

            modelBuilder.Entity("Models.DS_Bang", b =>
                {
                    b.Navigation("DS_Cap");
                });

            modelBuilder.Entity("Models.DS_Cap", b =>
                {
                    b.Navigation("DS_Trans");
                });

            modelBuilder.Entity("Models.DS_Giai", b =>
                {
                    b.Navigation("DS_Trinh");
                });

            modelBuilder.Entity("Models.DS_Trinh", b =>
                {
                    b.Navigation("DS_Bang");

                    b.Navigation("DS_Cap");

                    b.Navigation("DS_Tran");
                });

            modelBuilder.Entity("Models.DS_VDV", b =>
                {
                    b.Navigation("DS_Caps");

                    b.Navigation("DS_VDV_Diem");
                });

            modelBuilder.Entity("Models.Khu_Vuc", b =>
                {
                    b.Navigation("DS_Giai");

                    b.Navigation("Thong_Baos");
                });
#pragma warning restore 612, 618
        }
    }
}
