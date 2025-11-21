using LanguageApp.Api.Models;

namespace LanguageApp.Api.Services;

/// <summary>
/// Service để kiểm tra đáp án cho các loại bài tập khác nhau
/// </summary>
public static class AnswerValidationService
{
    /// <summary>
    /// Kiểm tra đáp án của học sinh có đúng không
    /// </summary>
    public static bool ValidateAnswer(CauHoiTracNghiem exercise, string userAnswer)
    {
        if (string.IsNullOrWhiteSpace(exercise.DapAnDung))
        {
            return false;
        }

        var loaiCauHoi = exercise.LoaiCauHoi ?? "TRAC_NGHIEM";
        var correctAnswer = exercise.DapAnDung.Trim();

        return loaiCauHoi switch
        {
            "TRAC_NGHIEM" => ValidateTracNghiem(userAnswer, correctAnswer),
            "DICH_CAU" => ValidateDichCau(userAnswer, correctAnswer, exercise),
            "DIEN_VAO_CHO_TRONG" => ValidateDienVaoChoTrong(userAnswer, correctAnswer),
            "SAP_XEP_TU" => ValidateSapXepTu(userAnswer, correctAnswer),
            "CHON_CAP" => ValidateChonCap(userAnswer, correctAnswer),
            _ => false
        };
    }

    /// <summary>
    /// Kiểm tra đáp án trắc nghiệm (A, B, C, D)
    /// </summary>
    private static bool ValidateTracNghiem(string userAnswer, string correctAnswer)
    {
        return string.Equals(userAnswer.Trim().ToUpper(), correctAnswer.Trim().ToUpper(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kiểm tra đáp án dịch câu (so sánh chuỗi, có thể normalize)
    /// </summary>
    private static bool ValidateDichCau(string userAnswer, string correctAnswer, CauHoiTracNghiem exercise)
    {
        // Normalize: loại bỏ dấu câu, khoảng trắng thừa, chuyển về lowercase
        var normalizedUser = NormalizeText(userAnswer);
        var normalizedCorrect = NormalizeText(correctAnswer);

        // So sánh chính xác
        if (string.Equals(normalizedUser, normalizedCorrect, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Nếu có CauTienViet hoặc CauTienAnh, có thể so sánh với các đáp án thay thế
        var alternativeAnswers = new List<string> { normalizedCorrect };
        
        if (!string.IsNullOrWhiteSpace(exercise.CauTienViet))
        {
            alternativeAnswers.Add(NormalizeText(exercise.CauTienViet));
        }
        
        if (!string.IsNullOrWhiteSpace(exercise.CauTienAnh))
        {
            alternativeAnswers.Add(NormalizeText(exercise.CauTienAnh));
        }

        return alternativeAnswers.Any(alt => string.Equals(normalizedUser, alt, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Kiểm tra đáp án điền vào chỗ trống
    /// </summary>
    private static bool ValidateDienVaoChoTrong(string userAnswer, string correctAnswer)
    {
        // Có thể có nhiều đáp án đúng (phân tách bằng dấu |)
        var correctAnswers = correctAnswer.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(a => NormalizeText(a))
            .ToList();

        var normalizedUser = NormalizeText(userAnswer);
        return correctAnswers.Any(correct => string.Equals(normalizedUser, correct, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Kiểm tra đáp án sắp xếp từ (so sánh thứ tự từ)
    /// </summary>
    private static bool ValidateSapXepTu(string userAnswer, string correctAnswer)
    {
        // Normalize và so sánh chuỗi từ (có thể có nhiều cách sắp xếp đúng)
        var normalizedUser = NormalizeText(userAnswer);
        var normalizedCorrect = NormalizeText(correctAnswer);

        // So sánh chính xác
        if (string.Equals(normalizedUser, normalizedCorrect, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Nếu đáp án có nhiều cách sắp xếp (phân tách bằng |)
        var correctVariants = correctAnswer.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(a => NormalizeText(a))
            .ToList();

        return correctVariants.Any(variant => string.Equals(normalizedUser, variant, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Kiểm tra đáp án chọn cặp (format: "0-100,1-101" hoặc JSON)
    /// </summary>
    private static bool ValidateChonCap(string userAnswer, string correctAnswer)
    {
        // Format có thể là: "0-100,1-101" hoặc JSON array
        var normalizedUser = userAnswer.Trim();
        var normalizedCorrect = correctAnswer.Trim();

        // So sánh chính xác
        if (string.Equals(normalizedUser, normalizedCorrect, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Nếu là format "0-100,1-101", có thể có nhiều cách match đúng
        // Parse và so sánh các cặp
        try
        {
            var userPairs = ParsePairs(normalizedUser);
            var correctPairs = ParsePairs(normalizedCorrect);

            if (userPairs.Count != correctPairs.Count)
            {
                return false;
            }

            // Kiểm tra xem tất cả các cặp của user có trong correct không
            return userPairs.All(up => correctPairs.Any(cp => cp.left == up.left && cp.right == up.right));
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Parse chuỗi cặp thành danh sách (format: "0-100,1-101")
    /// </summary>
    private static List<(int left, int right)> ParsePairs(string pairsString)
    {
        var pairs = new List<(int, int)>();
        var pairStrings = pairsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pairString in pairStrings)
        {
            var parts = pairString.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out var left) && int.TryParse(parts[1].Trim(), out var right))
            {
                pairs.Add((left, right));
            }
        }

        return pairs;
    }

    /// <summary>
    /// Normalize text: loại bỏ dấu câu, khoảng trắng thừa, chuyển về lowercase
    /// </summary>
    private static string NormalizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        // Loại bỏ khoảng trắng thừa
        var normalized = text.Trim();
        
        // Loại bỏ các ký tự đặc biệt (có thể tùy chỉnh)
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ");
        
        return normalized.ToLowerInvariant();
    }

    /// <summary>
    /// Lấy thông báo giải thích khi trả lời sai
    /// </summary>
    public static string GetExplanation(CauHoiTracNghiem exercise, bool isCorrect)
    {
        if (isCorrect)
        {
            return "Chính xác! Bạn vừa củng cố thêm kiến thức.";
        }

        var loaiCauHoi = exercise.LoaiCauHoi ?? "TRAC_NGHIEM";
        var correctAnswer = exercise.DapAnDung ?? "";

        return loaiCauHoi switch
        {
            "TRAC_NGHIEM" => $"Chưa đúng rồi. Đáp án đúng là {correctAnswer}.",
            "DICH_CAU" => $"Chưa đúng rồi. Đáp án đúng là: {correctAnswer}",
            "DIEN_VAO_CHO_TRONG" => $"Chưa đúng rồi. Từ cần điền là: {correctAnswer.Split('|').FirstOrDefault() ?? correctAnswer}",
            "SAP_XEP_TU" => $"Chưa đúng rồi. Thứ tự đúng là: {correctAnswer.Split('|').FirstOrDefault() ?? correctAnswer}",
            "CHON_CAP" => "Chưa đúng rồi. Hãy thử lại các cặp từ.",
            _ => "Chưa đúng rồi. Hãy thử lại."
        };
    }
}


