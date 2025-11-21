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
            Exercises = lesson.CauHoiTracNghiems
                .Select(q => new ExerciseDto
                {
                    CauHoiId = q.CauHoiID,
                    LoaiCauHoi = q.LoaiCauHoi ?? "TRAC_NGHIEM",
                    NoiDung = q.NoiDung,
                    AudioURL = q.AudioURL,
                    CauTienViet = q.CauTienViet,
                    CauTienAnh = q.CauTienAnh,
                    PhuongAnA = q.PhuongAnA,
                    PhuongAnB = q.PhuongAnB,
                    PhuongAnC = q.PhuongAnC,
                    PhuongAnD = q.PhuongAnD,
                    DapAnDung = q.DapAnDung
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

        // Sử dụng AnswerValidationService để kiểm tra đáp án
        var correct = AnswerValidationService.ValidateAnswer(cauHoi, request.TraLoi);
        var explanation = AnswerValidationService.GetExplanation(cauHoi, correct);

        // Lưu đáp án (chỉ lưu ký tự đầu cho TRAC_NGHIEM, còn lại lưu đầy đủ)
        // Lưu ý: CauHoiHistory.TraLoi là char, nên chỉ lưu ký tự đầu
        char traLoiChar;
        if (cauHoi.LoaiCauHoi == "TRAC_NGHIEM" && request.TraLoi.Length > 0)
        {
            traLoiChar = request.TraLoi[0];
        }
        else if (request.TraLoi.Length > 0)
        {
            traLoiChar = request.TraLoi[0];
        }
        else
        {
            traLoiChar = '?';
        }

        var history = new CauHoiHistory
        {
            HocSinhID = hocSinh.HocSinhID,
            CauHoiID = cauHoi.CauHoiID,
            TraLoi = traLoiChar,
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
            Explanation = explanation,
            AwardedGems = awardedGems,
            AwardedEnergy = awardedEnergy,
            TotalGems = hocSinh.TongDiem ?? 0,
            TotalEnergy = hocSinh.NangLuongGioChoi ?? 0
        });
    }

    [HttpGet("progress/{hocSinhId:int}")]
    public async Task<ActionResult<Dictionary<int, LessonProgressDto>>> GetProgress(int hocSinhId)
    {
        var progressList = await _context.TienDos
            .Where(t => t.HocSinhID == hocSinhId)
            .Select(t => new LessonProgressDto
            {
                BaiHocId = t.BaiHocID ?? 0,
                SoLanHoanThanh = t.SoLanHoanThanh,
                DiemSo = t.DiemSo,
                NgayHoanThanh = t.NgayHoanThanh
            })
            .ToListAsync();

        var progressDict = progressList.ToDictionary(p => p.BaiHocId);

        return Ok(progressDict);
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

        // Tìm tiến độ hiện có (nếu có)
        var tienDo = await _context.TienDos.FirstOrDefaultAsync(t =>
            t.HocSinhID == request.HocSinhId && t.BaiHocID == request.BaiHocId);

        bool isFirstCompletion = false;
        bool isMastered = false;

        if (tienDo is null)
        {
            // Lần đầu hoàn thành
            tienDo = new TienDo
            {
                HocSinhID = request.HocSinhId,
                BaiHocID = request.BaiHocId,
                NgayHoanThanh = DateTime.UtcNow,
                DiemSo = request.DiemSo,
                SoLanHoanThanh = 1
            };
            _context.TienDos.Add(tienDo);
            isFirstCompletion = true;
        }
        else
        {
            // Đã hoàn thành ít nhất 1 lần
            if (tienDo.SoLanHoanThanh >= 2)
            {
                // Đã thông thạo rồi (hoàn thành 2 lần)
                return Ok(new { 
                    message = "Bạn đã thông thạo bài học này rồi! Hãy tiếp tục với bài học tiếp theo.",
                    isMastered = true,
                    soLanHoanThanh = tienDo.SoLanHoanThanh
                });
            }

            // Tăng số lần hoàn thành
            tienDo.SoLanHoanThanh += 1;
            tienDo.NgayHoanThanh = DateTime.UtcNow;
            if (request.DiemSo > (tienDo.DiemSo ?? 0))
            {
                tienDo.DiemSo = request.DiemSo; // Cập nhật điểm cao nhất
            }

            // Kiểm tra xem đã đạt "thông thạo" chưa (2 lần)
            if (tienDo.SoLanHoanThanh >= 2)
            {
                isMastered = true;
            }
        }

        // Tính toán Đá Quý dựa trên số tim còn lại
        int gemsAwarded = 0;
        switch (request.RemainingHearts)
        {
            case 3:
                gemsAwarded = 15;
                break;
            case 2:
                gemsAwarded = 10;
                break;
            case 1:
                gemsAwarded = 5;
                break;
            default:
                gemsAwarded = 0;
                break;
        }

        // Thưởng thêm nếu đạt thông thạo (hoàn thành lần thứ 2)
        if (isMastered)
        {
            gemsAwarded += 20; // Thưởng thêm cho việc thông thạo
        }

        // Cộng Đá Quý vào TongDiem
        hocSinh.TongDiem = (hocSinh.TongDiem ?? 0) + gemsAwarded;

        await _context.SaveChangesAsync();

        string message;
        if (isMastered)
        {
            message = $"🎉 Chúc mừng! Bạn đã thông thạo \"{lesson.TenBaiHoc}\"! Nhận được {gemsAwarded} 💎!";
        }
        else if (isFirstCompletion)
        {
            message = gemsAwarded > 0
                ? $"Tuyệt vời! Bạn nhận được {gemsAwarded} 💎! Hoàn thành thêm 1 lần nữa để thông thạo."
                : $"Bạn đã hoàn thành \"{lesson.TenBaiHoc}\"! Hoàn thành thêm 1 lần nữa để thông thạo.";
        }
        else
        {
            message = gemsAwarded > 0
                ? $"Tuyệt vời! Bạn nhận được {gemsAwarded} 💎! Còn {2 - tienDo.SoLanHoanThanh} lần nữa để thông thạo."
                : $"Bạn đã hoàn thành \"{lesson.TenBaiHoc}\" lần {tienDo.SoLanHoanThanh}!";
        }

        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh, message);
        return Ok(status);
    }
}

