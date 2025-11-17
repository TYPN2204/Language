using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("HocSinh")]
[Index("TenDangNhap", Name = "UQ__HocSinh__55F68FC08096B48C", IsUnique = true)]
[Index("Email", Name = "UQ__HocSinh__A9D105343B176E46", IsUnique = true)]
public partial class HocSinh
{
    [Key]
    public int HocSinhID { get; set; }

    [StringLength(50)]
    public string TenDangNhap { get; set; } = null!;

    [StringLength(255)]
    public string MatKhauHash { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    public int? TongDiem { get; set; }

    public int? NangLuongGioChoi { get; set; }

    [InverseProperty("HocSinh")]
    public virtual ICollection<BangXepHang> BangXepHangs { get; set; } = new List<BangXepHang>();

    [InverseProperty("HocSinh")]
    public virtual ICollection<HocSinh_PhanThuong> HocSinh_PhanThuongs { get; set; } = new List<HocSinh_PhanThuong>();

    [InverseProperty("HocSinh")]
    public virtual ICollection<TienDo> TienDos { get; set; } = new List<TienDo>();

    [InverseProperty("HocSinh")]
    public virtual ICollection<CauHoiHistory> CauHoiHistories { get; set; } = new List<CauHoiHistory>();

    [ForeignKey("HocSinhID")]
    [InverseProperty("HocSinhs")]
    public virtual ICollection<PhuHuynh> PhuHuynhs { get; set; } = new List<PhuHuynh>();
}
