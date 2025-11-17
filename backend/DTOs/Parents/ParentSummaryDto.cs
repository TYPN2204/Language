namespace LanguageApp.Api.DTOs.Parents;

public class ParentSummaryDto
{
    public int PhuHuynhId { get; set; }

    public string TenPhuHuynh { get; set; } = null!;

    public string? Email { get; set; }

    public string? ZaloId { get; set; }

    public IReadOnlyCollection<ChildSnapshotDto> Children { get; set; } = Array.Empty<ChildSnapshotDto>();

    public string SummaryText { get; set; } = string.Empty;
}

