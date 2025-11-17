using System.Text;
using LanguageApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Services;

public class LessonKnowledgeService
{
    private readonly LanguageAppDbContext _context;

    public LessonKnowledgeService(LanguageAppDbContext context)
    {
        _context = context;
    }

    public async Task<string> BuildKnowledgeAsync()
    {
        var builder = new StringBuilder();
        var courses = await _context.KhoaHocs
            .Include(k => k.BaiHocs)
            .ThenInclude(b => b.CauHoiTracNghiems)
            .ToListAsync();

        foreach (var course in courses)
        {
            builder.AppendLine($"Khóa học: {course.TenKhoaHoc} (độ khó {course.DoKho ?? 1})");
            if (!string.IsNullOrWhiteSpace(course.MoTa))
            {
                builder.AppendLine($"- Mô tả: {course.MoTa}");
            }

            foreach (var lesson in course.BaiHocs)
            {
                builder.AppendLine($"  Bài học: {lesson.TenBaiHoc}");
                foreach (var question in lesson.CauHoiTracNghiems.Take(2))
                {
                    builder.AppendLine($"   • Câu hỏi: {question.NoiDung}");
                    builder.AppendLine($"     Đáp án đúng: {question.DapAnDung}");
                }
            }
        }

        return builder.ToString();
    }

    public async Task<string?> GetLessonNameAsync(int? baiHocId)
    {
        if (baiHocId is null)
        {
            return null;
        }

        return await _context.BaiHocs
            .Where(b => b.BaiHocID == baiHocId)
            .Select(b => b.TenBaiHoc)
            .FirstOrDefaultAsync();
    }
}

