using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.WebUI.ExternalService.StorageService;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{

    public class CommentController : Controller
    {
        private readonly ICommentApiService _commentApiService;

        private readonly IUserService _userService;

        public CommentController(ICommentApiService commentApiService, IUserService userService)
        {
            _commentApiService = commentApiService;

            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsByPostId([FromQuery] string postId)
        {
            try
            {
                var response = await _commentApiService.GetCommentsByPostIdAsync(postId);
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Couldn't get comments" });
                }
                var comments = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>();
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


            model.UserName = _userService.GetUser().UserName;
            model.UserId = _userService.GetUserId();
            Console.WriteLine(model.UserName);
            Console.WriteLine(model.Content);
            Console.WriteLine(model.PostId);
            Console.WriteLine(model.UserId);
            if (string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.PostId))
            {
                return Json(new { success = false, message = "Geçersiz form verisi" });
            }

            try
            {
                var response = await _commentApiService.AddCommentAsync(model);

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

                var addedComment = await response.Content.ReadFromJsonAsync<CommentViewModel>();
                return Json(new { success = true, comment = addedComment });
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

        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromQuery] string commentId)
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