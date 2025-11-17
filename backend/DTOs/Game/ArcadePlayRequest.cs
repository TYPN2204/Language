using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Game;

public class ArcadePlayRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Range(5, 100)]
    public int EnergySpent { get; set; }
}

