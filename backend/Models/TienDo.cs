using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("TienDo")]
public partial class TienDo
{
    [Key]
    public int TienDoID { get; set; }

    public int? HocSinhID { get; set; }

    public int? BaiHocID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayHoanThanh { get; set; }

    public int? DiemSo { get; set; }

    [ForeignKey("BaiHocID")]
    [InverseProperty("TienDos")]
    public virtual BaiHoc? BaiHoc { get; set; }

    [ForeignKey("HocSinhID")]
    [InverseProperty("TienDos")]
    public virtual HocSinh? HocSinh { get; set; }
}
