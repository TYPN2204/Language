using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.Game;
using LanguageApp.Api.Models;
using LanguageApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public ShopController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("rewards")]
    public async Task<ActionResult<IEnumerable<RewardDto>>> GetRewards()
    {
        var rewards = await _context.PhanThuongs
            .OrderBy(r => r.Gia)
            .Select(r => new RewardDto
            {
                PhanThuongId = r.PhanThuongID,
                TenPhanThuong = r.TenPhanThuong,
                LoaiPhanThuong = r.LoaiPhanThuong,
                MoTa = r.MoTa,
                Gia = r.Gia,
                AssetUrl = r.AssetURL
            })
            .ToListAsync();

        return Ok(rewards);
    }

    [HttpPost("purchase")]
    public async Task<ActionResult<StudentStatusResponse>> Purchase(PurchaseRewardRequest request)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var reward = await _context.PhanThuongs.FirstOrDefaultAsync(r => r.PhanThuongID == request.PhanThuongId);
        if (reward is null)
        {
            return NotFound(new { message = "Vật phẩm không tồn tại." });
        }

        var currentGems = hocSinh.TongDiem ?? 0;
        if (currentGems < reward.Gia)
        {
            return BadRequest(new { message = "Bạn chưa đủ 💎 để mua vật phẩm này." });
        }

        hocSinh.TongDiem = currentGems - reward.Gia;

        _context.HocSinh_PhanThuongs.Add(new HocSinh_PhanThuong
        {
            HocSinhID = request.HocSinhId,
            PhanThuongID = reward.PhanThuongID,
            NgayNhan = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        var message = $"Bạn đã mua thành công \"{reward.TenPhanThuong}\"!";
        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh, message);
        return Ok(status);
    }

    [HttpPost("buy-ticket")]
    public async Task<ActionResult<StudentStatusResponse>> BuyTicket(BuyTicketRequest request)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var totalCost = GameBalance.TicketPriceGems * request.Quantity;
        var currentGems = hocSinh.TongDiem ?? 0;

        if (currentGems < totalCost)
        {
            return BadRequest(new { message = $"Bạn chưa đủ 💎 để mua {request.Quantity} vé. Cần {totalCost} 💎 (hiện có: {currentGems} 💎)." });
        }

        hocSinh.TongDiem = currentGems - totalCost;
        hocSinh.SoVeChoiGame = (hocSinh.SoVeChoiGame ?? 0) + request.Quantity;

        await _context.SaveChangesAsync();

        var message = $"Bạn đã mua thành công {request.Quantity} vé chơi game! (Đã trừ {totalCost} 💎)";
        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh, message);
        return Ok(status);
    }
}

