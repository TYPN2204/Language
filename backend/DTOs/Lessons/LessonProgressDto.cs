namespace LanguageApp.Api.DTOs.Lessons;

/// <summary>
/// DTO cho tiến độ của một bài học
/// </summary>
public class LessonProgressDto
{
    public int BaiHocId { get; set; }

    /// <summary>
    /// Số lần đã hoàn thành (0 = chưa làm, 1 = đã làm 1 lần, 2 = đã thông thạo)
    /// </summary>
    public int SoLanHoanThanh { get; set; }

    /// <summary>
    /// Điểm số cao nhất đạt được
    /// </summary>
    public int? DiemSo { get; set; }

    /// <summary>
    /// Ngày hoàn thành lần cuối
    /// </summary>
    public DateTime? NgayHoanThanh { get; set; }

    /// <summary>
    /// Đã thông thạo (SoLanHoanThanh >= 2)
    /// </summary>
    public bool IsMastered => SoLanHoanThanh >= 2;

    /// <summary>
    /// Đã hoàn thành ít nhất 1 lần
    /// </summary>
    public bool IsCompleted => SoLanHoanThanh >= 1;
}

