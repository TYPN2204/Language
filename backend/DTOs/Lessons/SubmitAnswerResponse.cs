namespace LanguageApp.Api.DTOs.Lessons;

public class SubmitAnswerResponse
{
    public bool Correct { get; set; }

    public string Explanation { get; set; } = string.Empty;

    public int AwardedGems { get; set; }

    public int AwardedEnergy { get; set; }

    public int TotalGems { get; set; }

    public int TotalEnergy { get; set; }
}

