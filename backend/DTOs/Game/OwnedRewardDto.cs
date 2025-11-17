namespace LanguageApp.Api.DTOs.Game;

public class OwnedRewardDto
{
    public int PhanThuongId { get; set; }

    public string TenPhanThuong { get; set; } = null!;

    public string LoaiPhanThuong { get; set; } = null!;

    public string? AssetUrl { get; set; }

    public int SoLanSoHuu { get; set; }
}

