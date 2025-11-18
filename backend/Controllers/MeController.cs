using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public MeController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("tickets")]
    public async Task<ActionResult<TicketResponse>> GetTickets([FromQuery] int hocSinhId)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == hocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        return Ok(new TicketResponse
        {
            SoVeChoiGame = hocSinh.SoVeChoiGame ?? 0,
            Message = null
        });
    }
}

