using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Game;

public class CompleteLessonRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Required]
    public int BaiHocId { get; set; }

    [Range(0, 100)]
    public int DiemSo { get; set; } = 0;
}

