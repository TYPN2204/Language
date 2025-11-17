using System.Collections.Generic;

namespace LanguageApp.Api.DTOs.Lessons;

public class LessonDetailResponse
{
    public int BaiHocId { get; set; }

    public string TenBaiHoc { get; set; } = null!;

    public string? CourseName { get; set; }

    public int QuestionCount => Questions.Count;

    public List<QuizQuestionDto> Questions { get; set; } = new();
}

