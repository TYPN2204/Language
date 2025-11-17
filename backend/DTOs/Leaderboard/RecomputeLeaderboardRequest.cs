using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Leaderboard;

public class RecomputeLeaderboardRequest
{
    [Range(1, 12)]
    public int? Month { get; set; }

    [Range(2020, 2100)]
    public int? Year { get; set; }
}

