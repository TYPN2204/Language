namespace LanguageApp.Api.DTOs.Auth;

public class AuthResponse
{
    public int HocSinhId { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string? Email { get; set; }

    public int TongDiem { get; set; }

    public int NangLuongGioChoi { get; set; }

    public string AccessToken { get; set; } = null!;
}

