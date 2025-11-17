namespace LanguageApp.Api.DTOs.Game;

public class StudentStatusResponse
{
    public int HocSinhId { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string? Email { get; set; }

    public int TongDiem { get; set; }

    public int NangLuongGioChoi { get; set; }

    public int CompletedLessons { get; set; }

    public IReadOnlyCollection<OwnedRewardDto> Inventory { get; set; } = Array.Empty<OwnedRewardDto>();

    public string? Message { get; set; }
}

