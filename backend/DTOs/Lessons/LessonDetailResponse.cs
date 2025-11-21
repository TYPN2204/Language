using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageApp.Api.DTOs.Lessons;

public class LessonDetailResponse
{
    public int BaiHocId { get; set; }

    public string TenBaiHoc { get; set; } = null!;

    public string? CourseName { get; set; }

    public int QuestionCount => Exercises.Count;

    /// <summary>
    /// Danh sách các bài tập đa dạng (thay thế Questions cũ)
    /// </summary>
    public List<ExerciseDto> Exercises { get; set; } = new();

    /// <summary>
    /// Giữ lại Questions để tương thích ngược (deprecated, sẽ bị xóa trong tương lai)
    /// </summary>
    [Obsolete("Sử dụng Exercises thay thế. Property này sẽ bị xóa trong tương lai.")]
    public List<QuizQuestionDto> Questions
    {
        get
        {
            // Chuyển đổi Exercises sang Questions để tương thích ngược
            return Exercises
                .Where(e => e.LoaiCauHoi == "TRAC_NGHIEM")
                .Select(e => new QuizQuestionDto
                {
                    CauHoiId = e.CauHoiId,
                    NoiDung = e.NoiDung,
                    PhuongAnA = e.PhuongAnA ?? "",
                    PhuongAnB = e.PhuongAnB ?? "",
                    PhuongAnC = e.PhuongAnC ?? "",
                    PhuongAnD = e.PhuongAnD ?? ""
                })
                .ToList();
        }
    }
}

