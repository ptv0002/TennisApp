﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace DataAccess
{
    public class TennisContext : IdentityDbContext<AppUser>
    {
        public TennisContext(DbContextOptions<TennisContext> options) : base(options) {}
        public virtual DbSet<DS_Cap> DS_Caps { get; set; }
        public virtual DbSet<DS_Diem> DS_Diems { get; set; }
        public virtual DbSet<DS_Giai> DS_Giais { get; set; }
        public virtual DbSet<DS_ThongBao> DS_ThongBaos { get; set; }
        public virtual DbSet<DS_Tran> DS_Trans { get; set; }
        public virtual DbSet<DS_Trinh> DS_Trinhs { get; set; }
        public virtual DbSet<DS_VDV> DS_VDVs { get; set; }
        public virtual DbSet<DS_Vong> DS_Vongs { get; set; }
        public virtual DbSet<Khu_Vuc> Khu_Vucs { get; set; }
        public virtual DbSet<DS_Bang> DS_Bangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }
            modelBuilder.Entity<DS_Diem>()
                .Property(c=>c.Diem).HasColumnType("decimal(5,3)");
            modelBuilder.Entity<DS_Bang>()
                .Property(c => c.Diem).HasColumnType("decimal(5,3)");
            modelBuilder.Entity<DS_Cap>()
                .Property(c => c.Diem).HasColumnType("decimal(5,3)");
        }
    }
}

