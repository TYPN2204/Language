using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

public partial class LanguageAppDbContext : DbContext
{
    public LanguageAppDbContext()
    {
    }

    public LanguageAppDbContext(DbContextOptions<LanguageAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiHoc> BaiHocs { get; set; }

    public virtual DbSet<BangXepHang> BangXepHangs { get; set; }

    public virtual DbSet<BaoCaoZalo> BaoCaoZalos { get; set; }

    public virtual DbSet<CauHoiTracNghiem> CauHoiTracNghiems { get; set; }

    public virtual DbSet<HocSinh> HocSinhs { get; set; }

    public virtual DbSet<CauHoiHistory> CauHoiHistories { get; set; }

    public virtual DbSet<HocSinh_PhanThuong> HocSinh_PhanThuongs { get; set; }

    public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }

    public virtual DbSet<PhanThuong> PhanThuongs { get; set; }

    public virtual DbSet<PhuHuynh> PhuHuynhs { get; set; }

    public virtual DbSet<TienDo> TienDos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:LanguageAppDb");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiHoc>(entity =>
        {
            entity.HasKey(e => e.BaiHocID).HasName("PK__BaiHoc__59827F5A8346D304");

            entity.HasOne(d => d.KhoaHoc).WithMany(p => p.BaiHocs).HasConstraintName("FK__BaiHoc__KhoaHocI__47DBAE45");
        });

        modelBuilder.Entity<BangXepHang>(entity =>
        {
            entity.HasKey(e => e.BangXepHangID).HasName("PK__BangXepH__B10B409D567B6353");

            entity.HasOne(d => d.HocSinh).WithMany(p => p.BangXepHangs).HasConstraintName("FK__BangXepHa__HocSi__5812160E");
        });

        modelBuilder.Entity<BaoCaoZalo>(entity =>
        {
            entity.HasKey(e => e.BaoCaoID).HasName("PK__BaoCaoZa__46CCFEA31372E37D");

            entity.HasOne(d => d.PhuHuynh).WithMany(p => p.BaoCaoZalos).HasConstraintName("FK__BaoCaoZal__PhuHu__5AEE82B9");
        });

        modelBuilder.Entity<CauHoiTracNghiem>(entity =>
        {
            entity.HasKey(e => e.CauHoiID).HasName("PK__CauHoiTr__EDF63FFC53ACAE0E");

            entity.Property(e => e.DapAnDung).IsFixedLength();

            entity.HasOne(d => d.BaiHoc).WithMany(p => p.CauHoiTracNghiems).HasConstraintName("FK__CauHoiTra__BaiHo__4AB81AF0");
        });

        modelBuilder.Entity<CauHoiHistory>(entity =>
        {
            entity.HasKey(e => e.CauHoiHistoryID);

            entity.Property(e => e.TraLoi).IsFixedLength();

            entity.HasOne(d => d.HocSinh)
                .WithMany(p => p.CauHoiHistories)
                .HasForeignKey(d => d.HocSinhID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.CauHoi)
                .WithMany()
                .HasForeignKey(d => d.CauHoiID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HocSinh>(entity =>
        {
            entity.HasKey(e => e.HocSinhID).HasName("PK__HocSinh__CD0A97BFE3D67AC0");

            entity.Property(e => e.NangLuongGioChoi).HasDefaultValue(0);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TongDiem).HasDefaultValue(0);
            entity.Property(e => e.SoVeChoiGame).HasDefaultValue(0);

            entity.HasMany(d => d.PhuHuynhs).WithMany(p => p.HocSinhs)
                .UsingEntity<Dictionary<string, object>>(
                    "HocSinh_PhuHuynh",
                    r => r.HasOne<PhuHuynh>().WithMany()
                        .HasForeignKey("PhuHuynhID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HocSinh_P__PhuHu__4316F928"),
                    l => l.HasOne<HocSinh>().WithMany()
                        .HasForeignKey("HocSinhID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HocSinh_P__HocSi__4222D4EF"),
                    j =>
                    {
                        j.HasKey("HocSinhID", "PhuHuynhID").HasName("PK__HocSinh___D0004AB62D69B51F");
                        j.ToTable("HocSinh_PhuHuynh");
                    });
        });

        modelBuilder.Entity<HocSinh_PhanThuong>(entity =>
        {
            entity.HasKey(e => e.HocSinhPhanThuongID).HasName("PK__HocSinh___D70F5923CB1A6827");

            entity.Property(e => e.NgayNhan).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.HocSinh).WithMany(p => p.HocSinh_PhanThuongs).HasConstraintName("FK__HocSinh_P__HocSi__534D60F1");

            entity.HasOne(d => d.PhanThuong).WithMany(p => p.HocSinh_PhanThuongs).HasConstraintName("FK__HocSinh_P__PhanT__5441852A");
        });

        modelBuilder.Entity<KhoaHoc>(entity =>
        {
            entity.HasKey(e => e.KhoaHocID).HasName("PK__KhoaHoc__AADD6C7295301612");
        });

        modelBuilder.Entity<PhanThuong>(entity =>
        {
            entity.HasKey(e => e.PhanThuongID).HasName("PK__PhanThuo__BCE28FE5676FBDA0");
        });

        modelBuilder.Entity<PhuHuynh>(entity =>
        {
            entity.HasKey(e => e.PhuHuynhID).HasName("PK__PhuHuynh__D0ADD090EDFD711D");
        });

        modelBuilder.Entity<TienDo>(entity =>
        {
            entity.HasKey(e => e.TienDoID).HasName("PK__TienDo__BC617641112A00D1");

            entity.HasOne(d => d.BaiHoc).WithMany(p => p.TienDos).HasConstraintName("FK__TienDo__BaiHocID__4E88ABD4");

            entity.HasOne(d => d.HocSinh).WithMany(p => p.TienDos).HasConstraintName("FK__TienDo__HocSinhI__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
