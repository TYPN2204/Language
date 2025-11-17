using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Game;

public class MatchingGameWinRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Required]
    [Range(5, 100)]
    public int EnergySpent { get; set; }

    [Required]
    [Range(1, 60)]
    public int TimeTaken { get; set; } // Thời gian hoàn thành game (giây)

    [Required]
    [Range(1, 20)]
    public int PairsMatched { get; set; } // Số cặp đã ghép
}

