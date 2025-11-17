using System.Net.Http.Json;
using LanguageApp.Api.Options;
using Microsoft.Extensions.Options;

namespace LanguageApp.Api.Services;

public class ChatbotBridge
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly ChatbotOptions _options;
    private readonly ILogger<ChatbotBridge> _logger;

    public ChatbotBridge(IHttpClientFactory httpFactory, IOptions<ChatbotOptions> options, ILogger<ChatbotBridge> logger)
    {
        _httpFactory = httpFactory;
        _options = options.Value;
        _logger = logger;
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_options.N8nWebhookUrl);

    public async Task<string?> AskAsync(string question, string knowledge, int? hocSinhId)
    {
        if (!IsConfigured)
        {
            return null;
        }

        try
        {
            using var client = _httpFactory.CreateClient();
            if (!string.IsNullOrWhiteSpace(_options.N8nApiKey))
            {
                client.DefaultRequestHeaders.Add("X-N8N-API-Key", _options.N8nApiKey);
            }

            var payload = new
            {
                question,
                knowledge,
                hocSinhId
            };

            var response = await client.PostAsJsonAsync(_options.N8nWebhookUrl, payload);
            response.EnsureSuccessStatusCode();
            var answer = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(answer) ? null : answer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Không thể gọi webhook n8n");
            return null;
        }
    }
}

