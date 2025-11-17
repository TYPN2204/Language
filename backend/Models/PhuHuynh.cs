using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("PhuHuynh")]
[Index("ZaloID", Name = "UQ__PhuHuynh__A74192233EF0C51C", IsUnique = true)]
[Index("Email", Name = "UQ__PhuHuynh__A9D105346BA04D33", IsUnique = true)]
public partial class PhuHuynh
{
    [Key]
    public int PhuHuynhID { get; set; }

    [StringLength(100)]
    public string TenPhuHuynh { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [StringLength(50)]
    public string? ZaloID { get; set; }

    [InverseProperty("PhuHuynh")]
    public virtual ICollection<BaoCaoZalo> BaoCaoZalos { get; set; } = new List<BaoCaoZalo>();

    [ForeignKey("PhuHuynhID")]
    [InverseProperty("PhuHuynhs")]
    public virtual ICollection<HocSinh> HocSinhs { get; set; } = new List<HocSinh>();
}
