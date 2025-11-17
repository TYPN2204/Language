using LanguageApp.Api.DTOs.Leaderboard;
using LanguageApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public LeaderboardController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("current")]
    public async Task<ActionResult<IEnumerable<LeaderboardEntryDto>>> GetCurrentLeaderboard()
    {
        var month = DateTime.UtcNow.Month;
        var year = DateTime.UtcNow.Year;

        var existing = await _context.BangXepHangs
            .Include(b => b.HocSinh)
            .Where(b => b.Thang == month && b.Nam == year)
            .OrderBy(b => b.ThuHang)
            .ToListAsync();

        if (existing.Count == 0)
        {
            await RecomputeInternalAsync(month, year);
            existing = await _context.BangXepHangs
                .Include(b => b.HocSinh)
                .Where(b => b.Thang == month && b.Nam == year)
                .OrderBy(b => b.ThuHang)
                .ToListAsync();
        }

        var dto = existing.Select(entry => new LeaderboardEntryDto
        {
            HocSinhId = entry.HocSinhID ?? 0,
            TenDangNhap = entry.HocSinh?.TenDangNhap ?? "Ẩn danh",
            Rank = entry.ThuHang ?? 0,
            TongDiemThang = entry.TongDiemThang ?? 0,
            TongDiem = entry.HocSinh?.TongDiem ?? 0
        });

        return Ok(dto);
    }

    [HttpPost("recompute")]
    public async Task<IActionResult> Recompute(RecomputeLeaderboardRequest request)
    {
        var targetMonth = request.Month ?? DateTime.UtcNow.Month;
        var targetYear = request.Year ?? DateTime.UtcNow.Year;

        await RecomputeInternalAsync(targetMonth, targetYear);
        return Ok(new { message = $"Đã cập nhật bảng xếp hạng {targetMonth}/{targetYear}." });
    }

    private async Task RecomputeInternalAsync(int month, int year)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        var monthlyScores = await _context.TienDos
            .Where(t => t.NgayHoanThanh >= start && t.NgayHoanThanh < end)
            .GroupBy(t => t.HocSinhID)
            .Select(group => new
            {
                HocSinhId = group.Key ?? 0,
                Score = group.Sum(t => t.DiemSo ?? 0)
            })
            .OrderByDescending(g => g.Score)
            .Take(50)
            .ToListAsync();

        var existing = await _context.BangXepHangs
            .Where(b => b.Thang == month && b.Nam == year)
            .ToListAsync();

        if (existing.Count > 0)
        {
            _context.BangXepHangs.RemoveRange(existing);
        }

        var rankEntries = monthlyScores
            .Select((entry, index) => new BangXepHang
            {
                HocSinhID = entry.HocSinhId,
                Thang = month,
                Nam = year,
                ThuHang = index + 1,
                TongDiemThang = entry.Score
            })
            .ToList();

        await _context.BangXepHangs.AddRangeAsync(rankEntries);
        await _context.SaveChangesAsync();
    }
}

