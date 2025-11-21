using System.ComponentModel.DataAnnotations;

namespace LanguageApp.Api.DTOs.Lessons;

public class SubmitAnswerRequest
{
    [Required]
    public int HocSinhId { get; set; }

    [Required]
    public int CauHoiId { get; set; }

    /// <summary>
    /// Đáp án của học sinh. Format tùy theo loại bài tập:
    /// - TRAC_NGHIEM: 'A', 'B', 'C', 'D'
    /// - DICH_CAU: chuỗi dịch
    /// - DIEN_VAO_CHO_TRONG: từ cần điền
    /// - SAP_XEP_TU: chuỗi từ đã sắp xếp
    /// - CHON_CAP: chuỗi cặp (format: "0-100,1-101")
    /// </summary>
    [Required]
    [StringLength(1000)] // Tăng độ dài để hỗ trợ các loại đáp án dài
    public string TraLoi { get; set; } = null!;
}

