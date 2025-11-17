using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("KhoaHoc")]
public partial class KhoaHoc
{
    [Key]
    public int KhoaHocID { get; set; }

    [StringLength(100)]
    public string TenKhoaHoc { get; set; } = null!;

    [StringLength(500)]
    public string? MoTa { get; set; }

    public int? DoKho { get; set; }

    [InverseProperty("KhoaHoc")]
    public virtual ICollection<BaiHoc> BaiHocs { get; set; } = new List<BaiHoc>();
}
