using LanguageApp.Api.DTOs.Parents;
using LanguageApp.Api.Models;
using LanguageApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParentsController : ControllerBase
{
    private readonly LanguageAppDbContext _context;

    public ParentsController(LanguageAppDbContext context)
    {
        _context = context;
    }

    [HttpPost("link")]
    public async Task<ActionResult<ParentSummaryDto>> LinkParent(LinkParentRequest request)
    {
        var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(h => h.HocSinhID == request.HocSinhId);
        if (hocSinh is null)
        {
            return NotFound(new { message = "Không tìm thấy học sinh để liên kết." });
        }

        var parent = await _context.PhuHuynhs
            .FirstOrDefaultAsync(p =>
                (request.Email != null && p.Email == request.Email) ||
                (request.ZaloId != null && p.ZaloID == request.ZaloId));

        if (parent is null)
        {
            parent = new PhuHuynh
            {
                TenPhuHuynh = request.TenPhuHuynh,
                Email = request.Email,
                SoDienThoai = request.SoDienThoai,
                ZaloID = request.ZaloId
            };
            _context.PhuHuynhs.Add(parent);
        }
        else
        {
            parent.TenPhuHuynh = request.TenPhuHuynh;
            parent.SoDienThoai = request.SoDienThoai ?? parent.SoDienThoai;
        }

        await _context.SaveChangesAsync();

        await _context.Entry(parent)
            .Collection(p => p.HocSinhs)
            .LoadAsync();

        var alreadyLinked = parent.HocSinhs.Any(h => h.HocSinhID == hocSinh.HocSinhID);
        if (!alreadyLinked)
        {
            parent.HocSinhs.Add(hocSinh);
            await _context.SaveChangesAsync();
        }

        var summary = await ParentReportBuilder.CreateAsync(_context, parent);
        return Ok(summary);
    }

    [HttpGet("{parentId:int}/summary")]
    public async Task<ActionResult<ParentSummaryDto>> GetSummary(int parentId)
    {
        var parent = await _context.PhuHuynhs.FirstOrDefaultAsync(p => p.PhuHuynhID == parentId);
        if (parent is null)
        {
            return NotFound(new { message = "Không tìm thấy phụ huynh." });
        }

        var summary = await ParentReportBuilder.CreateAsync(_context, parent);
        return Ok(summary);
    }
}

