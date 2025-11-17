using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("HocSinh_PhanThuong")]
public partial class HocSinh_PhanThuong
{
    [Key]
    public int HocSinhPhanThuongID { get; set; }

    public int? HocSinhID { get; set; }

    public int? PhanThuongID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayNhan { get; set; }

    [ForeignKey("HocSinhID")]
    [InverseProperty("HocSinh_PhanThuongs")]
    public virtual HocSinh? HocSinh { get; set; }

    [ForeignKey("PhanThuongID")]
    [InverseProperty("HocSinh_PhanThuongs")]
    public virtual PhanThuong? PhanThuong { get; set; }
}
