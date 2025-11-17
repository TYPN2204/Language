namespace LanguageApp.Api.DTOs.Parents;

public class ChildSnapshotDto
{
    public int HocSinhId { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public int TongDiem { get; set; }

    public int NangLuongGioChoi { get; set; }

    public int CompletedLessons { get; set; }

    public DateTime? LastLessonAt { get; set; }

    public int WeekLessonCount { get; set; }
}

