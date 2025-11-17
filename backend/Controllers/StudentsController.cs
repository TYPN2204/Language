using LanguageApp.Api.DTOs.Game;
using LanguageApp.Api.Models;
using LanguageApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public StudentsController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{hocSinhId:int}/status")]
    public async Task<ActionResult<StudentStatusResponse>> GetStatus(int hocSinhId)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == hocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh." });
        }

        var status = await StudentStatusFactory.CreateAsync(_context, hocSinh);
        return Ok(status);
    }
}

