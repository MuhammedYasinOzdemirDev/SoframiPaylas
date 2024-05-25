using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentApiService _commentApiService;
        private readonly IMapper _mapper;

        public CommentController(ICommentApiService commentApiService, IMapper mapper)
        {
            _commentApiService = commentApiService;
            _mapper = mapper;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            try
            {
                var commentDtos = await _commentApiService.GetCommentsByPostIdAsync(postId);
                var comments = _mapper.Map<List<CommentViewModel>>(commentDtos);
                return Json(new { success = true, comments });
            }
            catch (HttpRequestException ex)
            {
                return Json(new { success = false, message = $"Yorumları getirirken bir hata meydana geldi: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Beklenmedik bir hata meydana geldi: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentViewModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Model cannot be null." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz form verisi" });
            }

            try
            {
                var createCommentDto = _mapper.Map<CreateCommentDto>(model);
                var response = await _commentApiService.AddCommentAsync(createCommentDto);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json(new { success = false, message = "Giriş sırasında bir hata oluştu: " + errorMessage });
                    }
                    else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                    {
                        return Json(new { success = false, message = "Bir sunucu hatası oluştu, lütfen daha sonra tekrar deneyiniz." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Giriş sırasında bir hata oluştu: " + errorMessage });
                    }
                }

                var addedComment = await response.Content.ReadFromJsonAsync<CommentDto>();
                var commentViewModel = _mapper.Map<CommentViewModel>(addedComment);
                return Json(new { success = true, comment = commentViewModel });
            }
            catch (HttpRequestException ex)
            {
                return Json(new { success = false, message = $"Yorum eklenirken bir hata meydana geldi: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Beklenmedik bir hata meydana geldi: {ex.Message}" });
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            try
            {
                var response = await _commentApiService.DeleteCommentAsync(commentId);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = await response.Content.ReadAsStringAsync() });
            }
            catch (HttpRequestException ex)
            {
                return Json(new { success = false, message = $"Yorum silinirken bir hata meydana geldi: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Beklenmedik bir hata meydana geldi: {ex.Message}" });
            }
        }
    }
}