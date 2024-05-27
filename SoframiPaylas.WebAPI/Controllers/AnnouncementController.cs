namespace SoframiPaylas.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Announcement;
using SoframiPaylas.Application.Interfaces;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpPost("announce")]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto announcementDto)
    {
        try
        {
            if (announcementDto == null)
            {
                return BadRequest("Duyuru bilgileri eksik.");
            }

            var createdAnnouncement = await _announcementService.AddAnnouncementAsync(announcementDto);
            return CreatedAtAction(nameof(GetAnnouncements), new { id = createdAnnouncement.Id }, createdAnnouncement);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Duyuru oluşturulurken bir hata meydana geldi.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAnnouncements()
    {
        try
        {
            var announcements = await _announcementService.GetAnnouncementsAsync();
            if (announcements == null || !announcements.Any())
            {
                return NotFound("Hiç duyuru bulunamadı.");
            }
            return Ok(announcements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Duyurular getirilirken bir hata meydana geldi.");
        }
    }
}
