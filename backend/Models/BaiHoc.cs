using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("BaiHoc")]
public partial class BaiHoc
{
    [Key]
    public int BaiHocID { get; set; }

    public int? KhoaHocID { get; set; }

    [StringLength(150)]
    public string TenBaiHoc { get; set; } = null!;

    public int? ThuTu { get; set; }

    [InverseProperty("BaiHoc")]
    public virtual ICollection<CauHoiTracNghiem> CauHoiTracNghiems { get; set; } = new List<CauHoiTracNghiem>();

    [ForeignKey("KhoaHocID")]
    [InverseProperty("BaiHocs")]
    public virtual KhoaHoc? KhoaHoc { get; set; }

    [InverseProperty("BaiHoc")]
    public virtual ICollection<TienDo> TienDos { get; set; } = new List<TienDo>();
}
