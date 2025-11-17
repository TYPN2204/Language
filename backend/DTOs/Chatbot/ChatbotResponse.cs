namespace LanguageApp.Api.DTOs.Chatbot;

public class ChatbotResponse
{
    public string Answer { get; set; } = string.Empty;

    public string Source { get; set; } = "local";

    public string? LessonReference { get; set; }
}

