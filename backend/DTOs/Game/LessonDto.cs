namespace LanguageApp.Api.DTOs.Game;

public class LessonDto
{
    public int BaiHocId { get; set; }

    public int KhoaHocId { get; set; }

    public string TenBaiHoc { get; set; } = null!;

    public int? ThuTu { get; set; }
}

