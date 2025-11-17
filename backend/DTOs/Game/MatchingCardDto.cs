namespace LanguageApp.Api.DTOs.Game;

public class MatchingCardDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty; // Placeholder cho hình ảnh
    public int PairId { get; set; } // ID để ghép cặp
}

public class MatchingGameDataResponse
{
    public List<MatchingCardDto> Cards { get; set; } = new();
}

