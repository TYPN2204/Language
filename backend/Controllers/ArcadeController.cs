using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.Game;
using LanguageApp.Api.Models;
using LanguageApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArcadeController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public ArcadeController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("matching-game/data")]
    public async Task<ActionResult<MatchingGameDataResponse>> GetMatchingGameData()
    {
        // Lấy các từ vựng từ câu hỏi trắc nghiệm để tạo matching game
        var questions = await _context.CauHoiTracNghiems
            .Where(q => q.BaiHocID != null)
            .Take(8) // Lấy 8 câu hỏi = 4 cặp
            .ToListAsync();

        if (questions.Count < 4)
        {
            return BadRequest(new { message = "Không đủ dữ liệu để tạo game. Cần ít nhất 4 câu hỏi." });
        }

        var cards = new List<MatchingCardDto>();
        var pairId = 1;

        // Tạo cặp từ vựng từ các câu hỏi
        // Mỗi câu hỏi tạo 1 cặp: từ vựng và nghĩa (sử dụng các phương án)
        foreach (var question in questions.Take(4))
        {
            // Card 1: Từ vựng (sử dụng nội dung câu hỏi hoặc phương án A như từ vựng)
            cards.Add(new MatchingCardDto
            {
                Id = pairId * 2 - 1,
                Text = question.PhuongAnA.Length > 30 ? question.PhuongAnA.Substring(0, 30) : question.PhuongAnA,
                ImageUrl = $"🎯", // Emoji placeholder
                PairId = pairId
            });

            // Card 2: Nghĩa hoặc từ liên quan (sử dụng phương án B hoặc C)
            cards.Add(new MatchingCardDto
            {
                Id = pairId * 2,
                Text = question.DapAnDung == "A" 
                    ? (question.PhuongAnB.Length > 30 ? question.PhuongAnB.Substring(0, 30) : question.PhuongAnB)
                    : (question.PhuongAnA.Length > 30 ? question.PhuongAnA.Substring(0, 30) : question.PhuongAnA),
                ImageUrl = $"✨", // Emoji placeholder
                PairId = pairId
            });

            pairId++;
        }

        // Shuffle cards
        cards = cards.OrderBy(c => Guid.NewGuid()).ToList();

        return Ok(new MatchingGameDataResponse { Cards = cards });
    }

    [HttpPost("matching-game/win")]
    public async Task<ActionResult<StudentStatusResponse>> MatchingGameWin(MatchingGameWinRequest request)
    {
        if (request.EnergySpent % GameBalance.ArcadeEnergyStep != 0)
        {
            return BadRequest(new { message = $"Năng lượng sử dụng phải bội số của {GameBalance.ArcadeEnergyStep}." });
        }

        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var currentEnergy = hocSinh.NangLuongGioChoi ?? 0;
        if (request.EnergySpent > currentEnergy)
        {
            return BadRequest(new { message = "Bạn không đủ năng lượng để chơi." });
        }

        // Tính toán gem dựa trên thời gian và số cặp đã ghép
        // Thời gian càng nhanh, gem càng nhiều
        var baseGems = request.EnergySpent / 2; // Base: 1 gem cho mỗi 2% energy
        var timeBonus = Math.Max(0, 60 - request.TimeTaken); // Bonus cho tốc độ (max 60 giây)
        var speedMultiplier = 1 + (timeBonus / 100.0); // Tăng 1% cho mỗi giây nhanh hơn
        var pairBonus = request.PairsMatched * 2; // 2 gem cho mỗi cặp đã ghép

        var gemsEarned = (int)(baseGems * speedMultiplier) + pairBonus;

        hocSinh.NangLuongGioChoi = currentEnergy - request.EnergySpent;
        hocSinh.TongDiem = (hocSinh.TongDiem ?? 0) + gemsEarned;

        await _context.SaveChangesAsync();

        var message = $"🎉 Chúc mừng! Bạn đã thắng Matching Game và nhận được {gemsEarned} 💎! (Đã sử dụng {request.EnergySpent}% năng lượng)";
        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh, message);
        return Ok(status);
    }

    [HttpPost("use-ticket")]
    public async Task<ActionResult<TicketResponse>> UseTicket(UseTicketRequest request)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var currentTickets = hocSinh.SoVeChoiGame ?? 0;
        if (currentTickets < 1)
        {
            return BadRequest(new { message = "Bạn không có vé chơi game. Hãy mua vé tại cửa hàng!" });
        }

        hocSinh.SoVeChoiGame = currentTickets - 1;
        await _context.SaveChangesAsync();

        return Ok(new TicketResponse
        {
            SoVeChoiGame = hocSinh.SoVeChoiGame ?? 0,
            Message = "Bạn đã sử dụng 1 vé chơi game thành công!"
        });
    }

    // Deprecated: Giữ lại để backward compatibility nhưng không dùng nữa
    [HttpPost("play")]
    [Obsolete("Sử dụng matching-game/win thay vì endpoint này")]
    public async Task<ActionResult<StudentStatusResponse>> Play(ArcadePlayRequest request)
    {
        return BadRequest(new { message = "Endpoint này đã bị loại bỏ. Vui lòng chơi Matching Game để nhận thưởng." });
    }
}

