using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("CauHoiHistory")]
public class CauHoiHistory
{
    [Key]
    public int CauHoiHistoryID { get; set; }

    public int HocSinhID { get; set; }

    public int CauHoiID { get; set; }

    public char TraLoi { get; set; }

    public bool Dung { get; set; }

    public int Diem { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey(nameof(HocSinhID))]
    public virtual HocSinh HocSinh { get; set; } = null!;

    [ForeignKey(nameof(CauHoiID))]
    public virtual CauHoiTracNghiem CauHoi { get; set; } = null!;
}

