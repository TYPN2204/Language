using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Chatbot;

public class ChatbotRequest
{
    [Required]
    [MaxLength(1000)]
    public string Question { get; set; } = null!;

    public int? HocSinhId { get; set; }
}

