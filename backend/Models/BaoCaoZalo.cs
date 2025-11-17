using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Models;

[Table("BaoCaoZalo")]
public partial class BaoCaoZalo
{
    [Key]
    public int BaoCaoID { get; set; }

    public int? PhuHuynhID { get; set; }

    public string? NoiDungBaoCao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayGui { get; set; }

    [StringLength(50)]
    public string? TrangThai { get; set; }

    [ForeignKey("PhuHuynhID")]
    [InverseProperty("BaoCaoZalos")]
    public virtual PhuHuynh? PhuHuynh { get; set; }
}
