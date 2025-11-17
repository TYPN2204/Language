using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Services;

public static class StudentStatusFactory
{
    public static async Task<StudentStatusResponse> CreateAsync(LanguageAppDbContext context, HocSinh hocSinh, string? message = null)
    {
        var inventory = await context.HocSinh_PhanThuongs
            .Where(h => h.HocSinhID == hocSinh.HocSinhID && h.PhanThuongID != null)
            .GroupBy(h => h.PhanThuongID!.Value)
            .Select(group => new OwnedRewardDto
            {
                PhanThuongId = group.Key,
                TenPhanThuong = group.Select(x => x.PhanThuong!.TenPhanThuong).FirstOrDefault() ?? "Vật phẩm bí ẩn",
                LoaiPhanThuong = group.Select(x => x.PhanThuong!.LoaiPhanThuong).FirstOrDefault() ?? "Unknown",
                AssetUrl = group.Select(x => x.PhanThuong!.AssetURL).FirstOrDefault(),
                SoLanSoHuu = group.Count()
            })
            .ToListAsync();

        var completedLessons = await context.TienDos.CountAsync(t => t.HocSinhID == hocSinh.HocSinhID);

        return new StudentStatusResponse
        {
            HocSinhId = hocSinh.HocSinhID,
            TenDangNhap = hocSinh.TenDangNhap,
            Email = hocSinh.Email,
            TongDiem = hocSinh.TongDiem ?? 0,
            NangLuongGioChoi = hocSinh.NangLuongGioChoi ?? 0,
            CompletedLessons = completedLessons,
            Inventory = inventory,
            Message = message
        };
    }
}

