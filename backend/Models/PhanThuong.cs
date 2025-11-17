using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("PhanThuong")]
public partial class PhanThuong
{
    [Key]
    public int PhanThuongID { get; set; }

    [StringLength(100)]
    public string TenPhanThuong { get; set; } = null!;

    [StringLength(50)]
    public string LoaiPhanThuong { get; set; } = null!;

    [StringLength(500)]
    public string? MoTa { get; set; }

    public int Gia { get; set; }

    [StringLength(255)]
    public string? AssetURL { get; set; }

    [InverseProperty("PhanThuong")]
    public virtual ICollection<HocSinh_PhanThuong> HocSinh_PhanThuongs { get; set; } = new List<HocSinh_PhanThuong>();
}
