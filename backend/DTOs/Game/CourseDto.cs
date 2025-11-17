namespace LanguageApp.Api.DTOs.Game;

public class CourseDto
{
    public int KhoaHocId { get; set; }

    public string TenKhoaHoc { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? DoKho { get; set; }

    public IReadOnlyCollection<LessonDto> Lessons { get; set; } = Array.Empty<LessonDto>();
}

