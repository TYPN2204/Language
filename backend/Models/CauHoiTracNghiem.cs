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

    /// <summary>
    /// Loại câu hỏi: 'TRAC_NGHIEM', 'DIEN_VAO_CHO_TRONG', 'DICH_CAU', 'SAP_XEP_TU', 'CHON_CAP'
    /// </summary>
    [StringLength(50)]
    public string LoaiCauHoi { get; set; } = "TRAC_NGHIEM";

    /// <summary>
    /// URL file âm thanh cho bài tập nghe (dùng cho DIEN_VAO_CHO_TRONG)
    /// </summary>
    [StringLength(500)]
    public string? AudioURL { get; set; }

    /// <summary>
    /// Câu tiếng Việt (dùng cho DICH_CAU)
    /// </summary>
    public string? CauTienViet { get; set; }

    /// <summary>
    /// Câu tiếng Anh (dùng cho DICH_CAU)
    /// </summary>
    public string? CauTienAnh { get; set; }

    [StringLength(255)]
    public string? PhuongAnA { get; set; }

    [StringLength(255)]
    public string? PhuongAnB { get; set; }

    [StringLength(255)]
    public string? PhuongAnC { get; set; }

    [StringLength(255)]
    public string? PhuongAnD { get; set; }

    /// <summary>
    /// Đáp án đúng. Với TRAC_NGHIEM là 'A','B','C','D'.
    /// Với các loại khác có thể là JSON string hoặc format khác.
    /// </summary>
    [StringLength(1)]
    [Unicode(false)]
    public string? DapAnDung { get; set; }

    [ForeignKey("BaiHocID")]
    [InverseProperty("CauHoiTracNghiems")]
    public virtual BaiHoc? BaiHoc { get; set; }
}
