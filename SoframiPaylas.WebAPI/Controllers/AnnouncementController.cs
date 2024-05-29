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
    /// <summary>
    /// Belirtilen Post ID'ye sahip bir duyuru oluşturur.
    /// </summary>
    /// <remarks>
    /// Bu işlem, belirtilen Post ID'ye sahip gönderiye yeni bir duyuru ekler.
    /// Eğer işlem başarılı olursa, 201 Created yanıtı döner.
    /// </remarks>
    /// <param name="announcementDto">Eklenmek üzere olan duyurunun detaylarını içeren DTO.</param>
    /// <param name="postId">Duyuru eklenecek gönderinin benzersiz tanımlayıcısı (ID).</param>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa yeni duyurunun detaylarını, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="201">Duyuru başarıyla eklendi.</response>
    /// <response code="400">Geçersiz istek. Duyuru bilgileri eksik.</response>
    /// <response code="500">Duyuru oluşturulurken bir hata meydana geldi.</response>
    [HttpPost("announce")]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto announcementDto, [FromQuery] string postId)
    {
        try
        {
            if (announcementDto == null)
            {
                return BadRequest("Duyuru bilgileri eksik.");
            }

            var createdAnnouncement = await _announcementService.AddAnnouncementAsync(postId, announcementDto);
            return CreatedAtAction(nameof(GetAnnouncements), new { id = createdAnnouncement.Id }, createdAnnouncement);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Duyuru oluşturulurken bir hata meydana geldi.");
        }
    }
    /// <summary>
    /// Sistemdeki tüm duyuruların listesini döner.
    /// </summary>
    /// <remarks>
    /// Bu işlem, sistemde kayıtlı tüm duyuruları getirir.
    /// Eğer kayıtlı hiç duyuru yoksa, kullanıcıya 404 hatası ile bilgi verilir.
    /// </remarks>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa duyuru listesini, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="200">Duyurular başarıyla bulundu ve döndürüldü.</response>
    /// <response code="404">Hiç duyuru bulunamadı.</response>
    /// <response code="500">Duyurular getirilirken bir hata meydana geldi.</response>
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
    /// <summary>
    /// Belirtilen Post ID'ye sahip tüm duyuruların listesini döner.
    /// </summary>
    /// <remarks>
    /// Bu işlem, belirtilen Post ID'ye sahip tüm duyuruları getirir.
    /// Eğer kayıtlı hiç duyuru yoksa, kullanıcıya 404 hatası ile bilgi verilir.
    /// </remarks>
    /// <param name="postId">Duyuruları getirilecek gönderinin benzersiz tanımlayıcısı (ID).</param>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa duyuru listesini, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="200">Duyurular başarıyla bulundu ve döndürüldü.</response>
    /// <response code="404">Hiç duyuru bulunamadı.</response>
    /// <response code="500">Duyurular getirilirken bir hata meydana geldi.</response>
    [HttpGet("post-id")]
    public async Task<IActionResult> GetPostIdAnnouncements([FromQuery] string postId)
    {
        try
        {
            var announcements = await _announcementService.GetPostIdAnnouncementsAsync(postId);
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
    /// <summary>
    /// Belirtilen Post ID'lere sahip tüm duyuruların listesini döner.
    /// </summary>
    /// <remarks>
    /// Bu işlem, belirtilen Post ID'lere sahip tüm duyuruları getirir.
    /// Eğer kayıtlı hiç duyuru yoksa, kullanıcıya 404 hatası ile bilgi verilir.
    /// </remarks>
    /// <param name="postIds">Duyuruları getirilecek gönderilerin benzersiz tanımlayıcılarının (ID) listesi.</param>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa duyuru listesini, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="200">Duyurular başarıyla bulundu ve döndürüldü.</response>
    /// <response code="404">Hiç duyuru bulunamadı.</response>
    /// <response code="500">Duyurular getirilirken bir hata meydana geldi.</response>
    [HttpPost("post-ids")]
    public async Task<IActionResult> GetPostIdsAnnouncements([FromBody] List<string> postIds)
    {
        try
        {
            var announcements = await _announcementService.GetPostIds(postIds);
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
