using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Game;

public class BuyTicketRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Range(1, 10)]
    public int Quantity { get; set; } = 1;
}

