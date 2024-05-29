namespace SoframiPaylas.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Message;
using SoframiPaylas.Application.Interfaces;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    /// <summary>
    /// Yeni bir mesaj gönderir.
    /// </summary>
    /// <remarks>
    /// Bu işlem, verilen mesaj bilgileri ile yeni bir mesaj oluşturur.
    /// Eğer işlem başarılı olursa, 201 Created yanıtı döner.
    /// </remarks>
    /// <param name="messageDto">Eklenmek üzere olan mesajın detaylarını içeren DTO.</param>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa yeni mesajın detaylarını, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="201">Mesaj başarıyla gönderildi.</response>
    /// <response code="400">Mesaj bilgileri eksik.</response>
    /// <response code="500">Mesaj oluşturulurken bir hata meydana geldi.</response>
    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] CreateMessageDto messageDto)
    {
        try
        {
            if (messageDto == null)
            {
                return BadRequest("Mesaj bilgileri eksik.");
            }

            var createdMessage = await _messageService.AddMessageAsync(messageDto);
            return CreatedAtAction(nameof(GetMessagesByReceiverId), new { receiverId = createdMessage.ReceiverId }, createdMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Mesaj oluşturulurken bir hata meydana geldi.");
        }
    }
    /// <summary>
    /// Belirtilen alıcı ID'sine sahip tüm mesajların listesini döner.
    /// </summary>
    /// <remarks>
    /// Bu işlem, belirtilen alıcı ID'sine sahip tüm mesajları getirir.
    /// Eğer kayıtlı hiç mesaj yoksa, kullanıcıya 404 hatası ile bilgi verilir.
    /// </remarks>
    /// <param name="receiverId">Mesajları getirilecek alıcının benzersiz tanımlayıcısı (ID).</param>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa mesaj listesini, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="200">Mesajlar başarıyla bulundu ve döndürüldü.</response>
    /// <response code="404">Hiç mesaj bulunamadı.</response>
    /// <response code="500">Mesajlar getirilirken bir hata meydana geldi.</response>
    [HttpGet]
    public async Task<IActionResult> GetMessagesByReceiverId([FromQuery] string receiverId)
    {
        try
        {
            var messages = await _messageService.GetMessagesByReceiverIdAsync(receiverId);
            if (messages == null || !messages.Any())
            {
                return NotFound("Hiç mesaj bulunamadı.");
            }
            return Ok(messages);
        }
        catch (Exception ex)
        {

            return StatusCode(500, "Mesajlar getirilirken bir hata meydana geldi.");
        }
    }
    /// <summary>
    /// Belirtilen kullanıcı ID'sine sahip tüm mesajların sayısını döner.
    /// </summary>
    /// <remarks>
    /// Bu işlem, belirtilen kullanıcı ID'sine sahip tüm mesajların sayısını getirir.
    /// </remarks>
    /// <param name="userId">Mesaj sayısı getirilecek kullanıcının benzersiz tanımlayıcısı (ID).</param>
    /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa mesaj sayısını, başarısız olursa hata mesajını içerir.</returns>
    /// <response code="200">Mesaj sayısı başarıyla bulundu ve döndürüldü.</response>
    /// <response code="500">Mesaj sayısı getirilirken bir hata meydana geldi.</response>
    [HttpGet("count")]
    public async Task<IActionResult> GetCountAsync([FromQuery] string userId)
    {
        try
        {
            var count = await _messageService.MessageCount(userId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Mesaj sayısı getirilirken bir hata meydana geldi.");
        }
    }
}
