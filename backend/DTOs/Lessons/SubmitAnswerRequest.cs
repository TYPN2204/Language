using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Lessons;

public class SubmitAnswerRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Required]
    public int CauHoiId { get; set; }

    [Required]
    [RegularExpression("^[ABCD]$", ErrorMessage = "Phương án phải là A, B, C hoặc D.")]
    public string TraLoi { get; set; } = null!;
}

