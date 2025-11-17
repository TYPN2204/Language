using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("CauHoiTracNghiem")]
public partial class CauHoiTracNghiem
{
    [Key]
    public int CauHoiID { get; set; }

    public int? BaiHocID { get; set; }

    public string NoiDung { get; set; } = null!;

    [StringLength(255)]
    public string PhuongAnA { get; set; } = null!;

    [StringLength(255)]
    public string PhuongAnB { get; set; } = null!;

    [StringLength(255)]
    public string PhuongAnC { get; set; } = null!;

    [StringLength(255)]
    public string PhuongAnD { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string DapAnDung { get; set; } = null!;

    [ForeignKey("BaiHocID")]
    [InverseProperty("CauHoiTracNghiems")]
    public virtual BaiHoc? BaiHoc { get; set; }
}
