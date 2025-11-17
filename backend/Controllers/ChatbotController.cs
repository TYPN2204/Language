using LanguageApp.Api.DTOs.Chatbot;
using LanguageApp.Api.Models;
using LanguageApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly LessonKnowledgeService _knowledgeService;
    private readonly ChatbotBridge _chatbotBridge;
    private readonly LanguageAppDbContext _context;

    public ChatbotController(LessonKnowledgeService knowledgeService, ChatbotBridge chatbotBridge, LanguageAppDbContext context)
    {
        _knowledgeService = knowledgeService;
        _chatbotBridge = chatbotBridge;
        _context = context;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<ChatbotResponse>> Ask(ChatbotRequest request)
    {
        var knowledge = await _knowledgeService.BuildKnowledgeAsync();
        string? lessonReference = null;

        if (request.HocSinhId.HasValue)
        {
            var lastLessonId = await _context.TienDos
                .Where(t => t.HocSinhID == request.HocSinhId)
                .OrderByDescending(t => t.NgayHoanThanh)
                .Select(t => t.BaiHocID)
                .FirstOrDefaultAsync();

            lessonReference = await _knowledgeService.GetLessonNameAsync(lastLessonId);
        }

        var externalAnswer = await _chatbotBridge.AskAsync(request.Question, knowledge, request.HocSinhId);
        if (!string.IsNullOrWhiteSpace(externalAnswer))
        {
            return Ok(new ChatbotResponse
            {
                Answer = externalAnswer,
                Source = "n8n",
                LessonReference = lessonReference
            });
        }

        var fallback = BuildLocalAnswer(request.Question, knowledge, lessonReference);
        return Ok(new ChatbotResponse
        {
            Answer = fallback,
            Source = "local",
            LessonReference = lessonReference
        });
    }

    private static string BuildLocalAnswer(string question, string knowledge, string? lessonReference)
    {
        var trimmedQuestion = question.Length > 120 ? question[..120] : question;
        var referenceText = lessonReference is null
            ? "Bạn có thể mở phần Trường học để xem lại các bài lý thuyết."
            : $"Hãy xem lại bài \"{lessonReference}\" trong Trường học để ôn tập thêm.";

        return $"Bạn hỏi: \"{trimmedQuestion}\".\n\nHiện tại mình chưa kết nối tới AI ngoài nên tạm thời dựa vào dữ liệu khóa học:\n{knowledge[..Math.Min(knowledge.Length, 500)]}...\n\n{referenceText}";
    }
}

