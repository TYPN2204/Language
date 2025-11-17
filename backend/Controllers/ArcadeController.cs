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

    [HttpPost("play")]
    public async Task<ActionResult<StudentStatusResponse>> Play(ArcadePlayRequest request)
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

        var multiplier = Random.Shared.Next(GameBalance.ArcadeRewardMultiplierMin, GameBalance.ArcadeRewardMultiplierMax + 1);
        var gemsEarned = request.EnergySpent * multiplier / 2;

        hocSinh.NangLuongGioChoi = currentEnergy - request.EnergySpent;
        hocSinh.TongDiem = (hocSinh.TongDiem ?? 0) + gemsEarned;

        await _context.SaveChangesAsync();

        var message = $"Bạn đã đổi {request.EnergySpent}% năng lượng và nhận được {gemsEarned} 💎!";
        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh, message);
        return Ok(status);
    }
}

