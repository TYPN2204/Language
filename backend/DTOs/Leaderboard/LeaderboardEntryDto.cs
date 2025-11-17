namespace LanguageApp.Api.DTOs.Leaderboard;

public class LeaderboardEntryDto
{
    public int HocSinhId { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public int Rank { get; set; }

    public int TongDiemThang { get; set; }

    public int TongDiem { get; set; }
}

