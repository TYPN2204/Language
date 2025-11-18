using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Game;

public class UseTicketRequest
{
    [Required]
    public int HocSinhId { get; set; }
}

