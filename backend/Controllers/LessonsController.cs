using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.DTOs.Lessons;
using LanguageApp.Api.Game;
using LanguageApp.Api.Models;
using LanguageApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public LessonsController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{lessonId:int}/detail")]
    public async Task<ActionResult<LessonDetailResponse>> GetLessonDetail(int lessonId)
    {
        var lesson = await _context.BaiHocs
            .Include(b => b.KhoaHoc)
            .Include(b => b.CauHoiTracNghiems)
            .FirstOrDefaultAsync(b => b.BaiHocID == lessonId);

        if (lesson is null)
        {
            return NotFound(new { message = "Không tìm thấy bài học." });
        }

        var response = new LessonDetailResponse
        {
            BaiHocId = lesson.BaiHocID,
            TenBaiHoc = lesson.TenBaiHoc,
            CourseName = lesson.KhoaHoc?.TenKhoaHoc ?? "Khóa học bí ẩn",
            Questions = lesson.CauHoiTracNghiems
                .Select(q => new QuizQuestionDto
                {
                    CauHoiId = q.CauHoiID,
                    NoiDung = q.NoiDung,
                    PhuongAnA = q.PhuongAnA,
                    PhuongAnB = q.PhuongAnB,
                    PhuongAnC = q.PhuongAnC,
                    PhuongAnD = q.PhuongAnD
                })
                .ToList()
        };

        return Ok(response);
    }

    [HttpPost("quiz")]
    public async Task<ActionResult<SubmitAnswerResponse>> SubmitAnswer(SubmitAnswerRequest request)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var cauHoi = await _context.CauHoiTracNghiems.FirstOrDefaultAsync(c => c.CauHoiID == request.CauHoiId);
        if (cauHoi is null)
        {
            return NotFound(new { message = "Câu hỏi không tồn tại." });
        }

        var alreadyCorrect = await _context.CauHoiHistories.AnyAsync(h =>
            h.HocSinhID == request.HocSinhId &&
            h.CauHoiID == request.CauHoiId &&
            h.Dung);

        var correct = string.Equals(request.TraLoi, cauHoi.DapAnDung, StringComparison.OrdinalIgnoreCase);

        var history = new CauHoiHistory
        {
            HocSinhID = hocSinh.HocSinhID,
            CauHoiID = cauHoi.CauHoiID,
            TraLoi = request.TraLoi[0],
            Dung = correct,
            Diem = correct ? 10 : 2
        };

        _context.CauHoiHistories.Add(history);

        var awardedGems = 0;
        var awardedEnergy = 0;

        if (correct && !alreadyCorrect)
        {
            awardedGems = 15;
            awardedEnergy = 5;
            hocSinh.TongDiem = (hocSinh.TongDiem ?? 0) + awardedGems;
            hocSinh.NangLuongGioChoi = Math.Min(GameBalance.MaxEnergy, (hocSinh.NangLuongGioChoi ?? 0) + awardedEnergy);
        }

        await _context.SaveChangesAsync();

        return Ok(new SubmitAnswerResponse
        {
            Correct = correct,
            Explanation = correct
                ? "Chính xác! Bạn vừa củng cố thêm kiến thức."
                : $"Chưa đúng rồi. Đáp án đúng là {cauHoi.DapAnDung}.",
            AwardedGems = awardedGems,
            AwardedEnergy = awardedEnergy,
            TotalGems = hocSinh.TongDiem ?? 0,
            TotalEnergy = hocSinh.NangLuongGioChoi ?? 0
        });
    }

    [HttpPost("complete")]
    public async Task<ActionResult<StudentStatusResponse>> CompleteLesson(CompleteLessonRequest request)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var lesson = await _context.BaiHocs.FirstOrDefaultAsync(b => b.BaiHocID == request.BaiHocId);
        if (lesson is null)
        {
            return NotFound(new { message = "Không tìm thấy bài học." });
        }

        var alreadyCompleted = await _context.TienDos.AnyAsync(t =>
            t.HocSinhID == request.HocSinhId && t.BaiHocID == request.BaiHocId);

        if (alreadyCompleted)
        {
            return Conflict(new { message = "Bạn đã hoàn thành bài học này rồi." });
        }

        var tienDo = new TienDo
        {
            HocSinhID = request.HocSinhId,
            BaiHocID = request.BaiHocId,
            NgayHoanThanh = DateTime.UtcNow,
            DiemSo = request.DiemSo
        };

        _context.TienDos.Add(tienDo);

        var currentEnergy = hocSinh.NangLuongGioChoi ?? 0;
        var updatedEnergy = Math.Min(GameBalance.MaxEnergy, currentEnergy + GameBalance.LessonEnergyGain);
        hocSinh.NangLuongGioChoi = updatedEnergy;

        await _context.SaveChangesAsync();

        var message = $"Bạn đã hoàn thành \"{lesson.TenBaiHoc}\" và nạp +{updatedEnergy - currentEnergy}% năng lượng!";
        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh, message);
        return Ok(status);
    }
}

