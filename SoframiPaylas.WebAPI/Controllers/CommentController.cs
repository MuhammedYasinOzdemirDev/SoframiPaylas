using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.WebAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Belirtilen Post ID'ye sahip tüm yorumların listesini döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen Post ID'ye sahip tüm yorumları getirir.
        /// Eğer kayıtlı hiç yorum yoksa, kullanıcıya 404 hatası ile bilgi verilir.
        /// </remarks>
        /// <param name="postId">Yorumları getirilecek gönderinin benzersiz tanımlayıcısı (ID).</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa yorum listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Yorumlar başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Hiç yorum bulunamadı.</response>
        /// <response code="500">Yorumları getirirken bir hata meydana geldi.</response>
        [HttpGet]
        public async Task<IActionResult> GetCommentsByPostId([FromQuery] string postId)
        {
            try
            {
                var comments = await _service.GetCommentsByPostIdAsync(postId);
                if (comments == null || !comments.Any())
                {
                    return NotFound("Hiç yorum bulunamadı.");
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Yorumları getirirken bir hata meydana geldi.");
            }
        }

        /// <summary>
        /// Yeni bir yorum ekler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen detaylarda yeni bir yorum ekler. Yorum bilgileri olarak PostID, UserID, UserName ve İçerik gereklidir.
        /// Başarılı bir oluşturma işlemi sonucunda, yorumun benzersiz tanımlayıcısı (ID) döndürülür.
        /// </remarks>
        /// <param name="commentDto">Eklenecek yorumun detaylarını içeren DTO nesnesi.</param>
        /// <returns>Yeni oluşturulan yorumun ID'si. Hata durumunda uygun hata mesajı içeren HTTP yanıtı döner.</returns>
        /// <response code="201">Yorum başarıyla oluşturuldu ve yorumun ID'si döndürüldü.</response>
        /// <response code="400">Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.</response>
        /// <response code="500">Yorum oluşturma işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto commentDto)
        {
            try
            {
                if (commentDto == null)
                {
                    return BadRequest("Yorum bilgileri eksik.");
                }

                var createdComment = await _service.AddCommentAsync(commentDto);
                return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = createdComment.PostId }, createdComment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Yorum oluşturulurken bir hata meydana geldi.");
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip olan yorumu sistemden siler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanında belirtilen ID'ye sahip yorumu bulur ve siler. Yorum bulunamazsa 404 hatası dönülür, silme başarılı olursa herhangi bir içerik dönmez (204 No Content).
        /// </remarks>
        /// <param name="commentId">Silinmek istenen yorumun benzersiz tanımlayıcısı (ID).</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise içerik dönmeyen yanıt, başarısız ise hata mesajı içerir.</returns>
        /// <response code="204">Yorum başarıyla silindi. İçerik dönmez.</response>
        /// <response code="404">Belirtilen ID'ye sahip yorum bulunamadı.</response>
        /// <response code="500">Silme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromQuery] string commentId)
        {
            try
            {
                var success = await _service.DeleteCommentAsync(commentId);
                if (success)
                {
                    return NoContent();
                }
                return NotFound("Silinecek yorum bulunamadı.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Yorum silinirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen kullanıcı ID'sine sahip tüm yorumların sayısını döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen kullanıcı ID'sine sahip tüm yorumların sayısını getirir.
        /// </remarks>
        /// <param name="userId">Yorum sayısı getirilecek kullanıcının benzersiz tanımlayıcısı (ID).</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa yorum sayısını, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Yorum sayısı başarıyla bulundu ve döndürüldü.</response>
        /// <response code="500">Yorum sayısı getirilirken bir hata meydana geldi.</response>
        [HttpGet("count")]
        public async Task<IActionResult> CommentCount([FromQuery] string userId)
        {
            try
            {
                var count = await _service.CommentCount(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Yorum sayısı getirilirken bir hata meydana geldi.");
            }
        }
    }
}
