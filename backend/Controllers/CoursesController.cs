using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public CoursesController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _context.KhoaHocs
            .Include(k => k.BaiHocs)
            .OrderBy(k => k.KhoaHocID)
            .ToListAsync();

        var result = courses.Select(course => new CourseDto
        {
            KhoaHocId = course.KhoaHocID,
            TenKhoaHoc = course.TenKhoaHoc,
            MoTa = course.MoTa,
            DoKho = course.DoKho,
            Lessons = course.BaiHocs
                .OrderBy(b => b.ThuTu)
                .Select(lesson =>
                {
                    var parentCourseId = course.KhoaHocID;
                    return new LessonDto
                    {
                        BaiHocId = lesson.BaiHocID,
                        KhoaHocId = lesson.KhoaHocID ?? parentCourseId,
                        TenBaiHoc = lesson.TenBaiHoc,
                        ThuTu = lesson.ThuTu
                    };
                })
                .ToList()
        });

        return Ok(result);
    }
}

