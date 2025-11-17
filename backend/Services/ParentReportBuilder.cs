using LanguageApp.Api.DTOs.Parents;
using LanguageApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Services;

public static class ParentReportBuilder
{
    public static async Task<ParentSummaryDto> CreateAsync(LanguageAppDbContext context, PhuHuynh parent)
    {
        await context.Entry(parent)
            .Collection(p => p.HocSinhs)
            .LoadAsync();

        var childIds = parent.HocSinhs.Select(h => h.HocSinhID).ToArray();

        var lessonStats = await context.TienDos
            .Where(t => childIds.Contains(t.HocSinhID ?? 0))
            .GroupBy(t => t.HocSinhID)
            .Select(group => new
            {
                HocSinhId = group.Key ?? 0,
                CompletedLessons = group.Count(),
                LastLessonAt = group.Max(t => t.NgayHoanThanh),
                WeekLessonCount = group.Count(t => t.NgayHoanThanh >= DateTime.UtcNow.AddDays(-7))
            })
            .ToListAsync();

        var lessonLookup = lessonStats.ToDictionary(k => k.HocSinhId);

        var children = parent.HocSinhs.Select(child =>
        {
            lessonLookup.TryGetValue(child.HocSinhID, out var stats);
            return new ChildSnapshotDto
            {
                HocSinhId = child.HocSinhID,
                TenDangNhap = child.TenDangNhap,
                TongDiem = child.TongDiem ?? 0,
                NangLuongGioChoi = child.NangLuongGioChoi ?? 0,
                CompletedLessons = stats?.CompletedLessons ?? 0,
                LastLessonAt = stats?.LastLessonAt,
                WeekLessonCount = stats?.WeekLessonCount ?? 0
            };
        }).ToList();

        var summaryLines = new List<string>
        {
            $"👋 Xin chào {parent.TenPhuHuynh}!",
            $"Bạn đang theo dõi {children.Count} học sinh.",
            string.Join(Environment.NewLine, children.Select(child =>
                $"- {child.TenDangNhap}: {child.CompletedLessons} bài học ({child.WeekLessonCount} trong 7 ngày), {child.TongDiem} 💎, năng lượng {child.NangLuongGioChoi}%"
            ))
        };

        return new ParentSummaryDto
        {
            PhuHuynhId = parent.PhuHuynhID,
            TenPhuHuynh = parent.TenPhuHuynh,
            Email = parent.Email,
            ZaloId = parent.ZaloID,
            Children = children,
            SummaryText = string.Join(Environment.NewLine, summaryLines.Where(line => !string.IsNullOrWhiteSpace(line)))
        };
    }
}

