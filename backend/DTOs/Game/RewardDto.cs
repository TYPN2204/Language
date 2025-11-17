namespace LanguageApp.Api.DTOs.Game;

public class RewardDto
{
    public int PhanThuongId { get; set; }

    public string TenPhanThuong { get; set; } = null!;

    public string LoaiPhanThuong { get; set; } = null!;

    public string? MoTa { get; set; }

    public int Gia { get; set; }

    public string? AssetUrl { get; set; }
}

