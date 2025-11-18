using LanguageApp.Api.DTOs.Auth;
using LanguageApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public AuthController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var normalizedUsername = request.TenDangNhap.Trim();
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        if (await _context.HocSinhs.AnyAsync(h => h.TenDangNhap == normalizedUsername))
        {
            return Conflict(new { message = "Tên đăng nhập đã tồn tại." });
        }

        if (await _context.HocSinhs.AnyAsync(h => h.Email == normalizedEmail))
        {
            return Conflict(new { message = "Email đã được sử dụng." });
        }

        var hocSinh = new HocSinh
        {
            TenDangNhap = normalizedUsername,
            Email = normalizedEmail,
            MatKhauHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            TongDiem = 0,
            NangLuongGioChoi = 0,
            SoVeChoiGame = 0,
            NgayTao = DateTime.UtcNow
        };

        _context.HocSinhs.Add(hocSinh);
        await _context.SaveChangesAsync();

        return Ok(BuildResponse(hocSinh));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var normalizedUsername = request.TenDangNhap.Trim();
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.TenDangNhap == normalizedUsername);

        if (hocSinh is null || !BCrypt.Net.BCrypt.Verify(request.Password, hocSinh.MatKhauHash))
        {
            return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu." });
        }

        return Ok(BuildResponse(hocSinh));
    }

    private static AuthResponse BuildResponse(HocSinh hocSinh) => new()
    {
        HocSinhId = hocSinh.HocSinhID,
        TenDangNhap = hocSinh.TenDangNhap,
        Email = hocSinh.Email,
        TongDiem = hocSinh.TongDiem ?? 0,
        NangLuongGioChoi = hocSinh.NangLuongGioChoi ?? 0,
        AccessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
    };
}

