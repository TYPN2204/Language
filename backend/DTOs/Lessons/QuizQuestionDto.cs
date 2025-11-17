namespace LanguageApp.Api.DTOs.Lessons;

public class QuizQuestionDto
{
    public int CauHoiId { get; set; }

    public string NoiDung { get; set; } = null!;

    public string PhuongAnA { get; set; } = null!;

    public string PhuongAnB { get; set; } = null!;

    public string PhuongAnC { get; set; } = null!;

    public string PhuongAnD { get; set; } = null!;
}

