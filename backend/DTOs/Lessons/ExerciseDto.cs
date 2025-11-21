namespace LanguageApp.Api.DTOs.Lessons;

/// <summary>
/// DTO cho câu hỏi/bài tập đa dạng, hỗ trợ nhiều loại bài tập khác nhau
/// </summary>
public class ExerciseDto
{
    public int CauHoiId { get; set; }

    /// <summary>
    /// Loại câu hỏi: 'TRAC_NGHIEM', 'DIEN_VAO_CHO_TRONG', 'DICH_CAU', 'SAP_XEP_TU', 'CHON_CAP'
    /// </summary>
    public string LoaiCauHoi { get; set; } = "TRAC_NGHIEM";

    /// <summary>
    /// Nội dung câu hỏi (có thể là câu hỏi, hướng dẫn, hoặc prompt)
    /// </summary>
    public string NoiDung { get; set; } = null!;

    /// <summary>
    /// URL file âm thanh (cho DIEN_VAO_CHO_TRONG)
    /// </summary>
    public string? AudioURL { get; set; }

    /// <summary>
    /// Câu tiếng Việt (cho DICH_CAU)
    /// </summary>
    public string? CauTienViet { get; set; }

    /// <summary>
    /// Câu tiếng Anh (cho DICH_CAU)
    /// </summary>
    public string? CauTienAnh { get; set; }

    // Các trường cho TRAC_NGHIEM (trắc nghiệm)
    public string? PhuongAnA { get; set; }
    public string? PhuongAnB { get; set; }
    public string? PhuongAnC { get; set; }
    public string? PhuongAnD { get; set; }

    /// <summary>
    /// Đáp án đúng (format tùy theo LoaiCauHoi)
    /// - TRAC_NGHIEM: 'A', 'B', 'C', 'D'
    /// - DICH_CAU: câu dịch đúng
    /// - DIEN_VAO_CHO_TRONG: từ cần điền
    /// - SAP_XEP_TU: thứ tự đúng (JSON array)
    /// - CHON_CAP: các cặp đúng (JSON array)
    /// </summary>
    public string? DapAnDung { get; set; }
}

