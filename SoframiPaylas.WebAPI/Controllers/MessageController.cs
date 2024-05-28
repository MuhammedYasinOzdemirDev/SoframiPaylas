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
