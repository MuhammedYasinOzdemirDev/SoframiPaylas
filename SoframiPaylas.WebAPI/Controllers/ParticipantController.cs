using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Participant;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }
        /// <summary>
        /// Bir kullanıcının belirtilen gönderiye katılma isteğini işler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen gönderiye bir kullanıcının katılma talebini kaydeder. Gönderi bulunamazsa 400 Bad Request hatası ile yanıt verilir. İşlem başarılı ise 204 No Content dönülür.
        ///
        /// ### Örnek İstek
        /// 
        ///     POST /api/Post/{postId}/join
        ///     {
        ///        "userID": "8ymayJzHVVX393V9ZdO8",
        ///        "status": 1
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Belirtilen ID'ye sahip gönderi bulunamadığında veya verilerde başka bir doğrulama hatası olduğunda bu hata dönülür.
        /// - **500 Internal Server Error**: Katılım işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="postId">Katılım isteğinin yapıldığı gönderinin benzersiz tanımlayıcısı.</param>
        /// <param name="joinRequest">Katılım isteği detaylarını içeren DTO nesnesi.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise içerik dönmeyen yanıt, başarısız ise hata mesajı içerir.</returns>
        /// <response code="204">Katılım isteği başarıyla işlendi.</response>
        /// <response code="400">Belirtilen gönderi bulunamadı veya veri doğrulaması başarısız oldu.</response>
        /// <response code="500">Katılım işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.</response>
        [HttpPost("join")]
        public async Task<IActionResult> RequestJoinEvent([FromBody] ParticipantDto joinRequest)
        {
            try
            {
                var joinResult = await _participantService.AddParticipantAsync(joinRequest);
                if (!joinResult)
                    return BadRequest("Belirtilen gönderi bulunamadı veya veri doğrulaması başarısız oldu.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılım işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        /// <summary>
        /// Belirtilen gönderi için bir katılımcıyı onaylar.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen gönderiye ve kullanıcıya ait katılım isteğinin durumunu günceller. Eğer katılımcı veya gönderi bulunamazsa, 404 Not Found hatası döner.
        ///
        /// ### Örnek İstek
        /// 
        ///     PUT /api/Post/{postId}/confirm-participant/{userId}
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı işlem sonucunda dönen yanıt:
        ///
        ///     {
        ///         "message": "Katılımcı başarıyla onaylandı."
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Belirtilen katılımcı veya gönderi bulunamadığında bu hata dönülür.
        /// - **500 Internal Server Error**: İşlem sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="postId">Onaylanacak katılımcının bağlı olduğu gönderinin benzersiz tanımlayıcısı.</param>
        /// <param name="userId">Onaylanacak katılımcının kullanıcı ID'si.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise onay mesajı, başarısız ise hata mesajı içerir.</returns>
        /// <response code="200">Katılımcı başarıyla onaylandı.</response>
        /// <response code="404">Belirtilen katılımcı veya gönderi bulunamadı.</response>
        /// <response code="500">Katılımcı onaylama işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.</response>
        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmParticipant([FromBody] ParticipantDto confirmRequest)
        {
            try
            {
                // Katılımcının durumunu güncelle
                var success = await _participantService.ConfirmedParticipantStatus(confirmRequest);

                if (!success)
                {
                    return NotFound(new { message = "Belirtilen katılımcı veya gönderi bulunamadı." });
                }

                return Ok(new { message = "Katılımcı başarıyla onaylandı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı onaylama işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        [HttpPut("decline")]
        public async Task<IActionResult> DeclinedParticipant([FromBody] ParticipantDto declineRequest)
        {
            try
            {
                // Katılımcının durumunu güncelle
                var success = await _participantService.DeclinedParticipantStatus(declineRequest);

                if (!success)
                {
                    return NotFound(new { message = "Belirtilen katılımcı veya gönderi bulunamadı." });
                }

                return Ok(new { message = "Katılımcı başarıyla reddedildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı onaylama işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        [HttpGet("pending-participants")]
        public async Task<IActionResult> PendingParticipants([FromQuery] string postId)
        {
            try
            {
                var participants = await _participantService.GetPendingPostIdByAsync(postId);
                if (participants == null || !participants.Any())
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(participants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı bilgileri getirilirken bir hata meydana geldi.");
            }
        }
        [HttpGet("confirm-participants")]
        public async Task<IActionResult> ConfirmedParticipants([FromQuery] string postId)
        {
            try
            {
                var participants = await _participantService.GetConfirmedPostIdByAsync(postId);
                if (participants == null || !participants.Any())
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(participants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı bilgileri getirilirken bir hata meydana geldi.");
            }
        }
        [HttpGet("check-status")]
        public async Task<IActionResult> CheckIfRequestExistsAsync([FromQuery] string postId, [FromQuery] string userId)
        {
            try
            {
                var status = await _participantService.CheckIfRequestExistsAsync(postId, userId);
                return Ok(status);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Status bilgileri getirilirken bir hata meydana geldi.");
            }
        }
        [HttpDelete("remove-participant")]
        public async Task<IActionResult> Delete([FromQuery] string participantId)
        {
            try
            {
                var deleteResult = await _participantService.DeleteParticipantAsync(participantId);
                if (!deleteResult)
                    return NotFound("Belirtilen ID'ye sahip katılımcı bulunamadı.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı silme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        [HttpGet("get-user-posts")]
        public async Task<IActionResult> GetPostIdAsync([FromQuery] string postId)
        {
            try
            {
                var users = await _participantService.GetPostIdAsync(postId);
                if (users == null || !users.Any())
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı getirme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }

        }
        [HttpDelete("leave-participant")]
        public async Task<IActionResult> Leave([FromQuery] string postId, [FromQuery] string userId)
        {
            try
            {
                var leaveResult = await _participantService.LeaveParticipantAsync(postId, userId);
                if (!leaveResult)
                    return NotFound("Belirtilen ID'ye sahip katılımcı bulunamadı.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Katılımcı silme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
    }
}