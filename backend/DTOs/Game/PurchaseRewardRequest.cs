using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Game;

public class PurchaseRewardRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Required]
    public int PhanThuongId { get; set; }
}

