using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Parents;

public class LinkParentRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Required]
    [MaxLength(100)]
    public string TenPhuHuynh { get; set; } = null!;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? SoDienThoai { get; set; }

    [MaxLength(50)]
    public string? ZaloId { get; set; }
}

