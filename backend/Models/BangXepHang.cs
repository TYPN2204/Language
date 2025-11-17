using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("BangXepHang")]
public partial class BangXepHang
{
    [Key]
    public int BangXepHangID { get; set; }

    public int? HocSinhID { get; set; }

    public int? Thang { get; set; }

    public int? Nam { get; set; }

    public int? ThuHang { get; set; }

    public int? TongDiemThang { get; set; }

    [ForeignKey("HocSinhID")]
    [InverseProperty("BangXepHangs")]
    public virtual HocSinh? HocSinh { get; set; }
}
