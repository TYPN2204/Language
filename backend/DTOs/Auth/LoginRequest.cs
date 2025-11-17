using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Auth;

public class LoginRequest
{
    [Required]
    [MaxLength(50)]
    public string TenDangNhap { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = null!;
}

