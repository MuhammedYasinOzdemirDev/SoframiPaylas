using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.WebUI.ExternalService.Filters;
using SoframiPaylas.WebUI.ExternalService.StorageService;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{

    public class ParticipantController : Controller
    {

        private readonly IUserService _userService;
        private readonly IParticipantApiService _apiservice;
        private readonly IPostApiService _postapiservice;

        public ParticipantController(IUserService userService, IParticipantApiService apiservice, IPostApiService postapiservice)
        {
            _apiservice = apiservice;
            _userService = userService;
            _postapiservice = postapiservice;
        }
        public async Task<IActionResult> Manage([FromQuery] string postId)
        {
            var response = await _apiservice.PendingParticipants(postId);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.PendingParticipants = await response.Content.ReadFromJsonAsync<List<ParticipantViewModel>>() ?? new List<ParticipantViewModel>();
            }
            else
            {
                ViewBag.PendingParticipants = new List<ParticipantViewModel>();
            }
            var response2 = await _apiservice.ConfirmParticipants(postId);

            if (response2.IsSuccessStatusCode)
            {
                ViewBag.ConfirmedParticipants = await response2.Content.ReadFromJsonAsync<List<ParticipantViewModel>>() ?? new List<ParticipantViewModel>();

            }
            else
            {
                ViewBag.ConfirmedParticipants = new List<ParticipantViewModel>();
            }
            var response3 = await _postapiservice.GetPostByIdAsync(postId);
            if (response3.IsSuccessStatusCode)
            {
                var content = await response3.Content.ReadFromJsonAsync<PostViewModel>();
                ViewBag.PostId = content.PostId;
                ViewBag.MaxParticipants = content.MaxParticipants;
            }
            else
            {
                View("Error");
            }


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Join([FromQuery] string postId)
        {
            try
            {
                var userId = _userService.GetUserId();
                HttpResponseMessage response = await _apiservice.Join(userId, postId);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json(new { success = false, message = "İstek sırasında bir hata oluştu: " + errorMessage });
                    }
                    else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                    {
                        // Sunucu hatası durumunda genel bir hata mesajı göster
                        return Json(new { success = false, message = "Bir sunucu hatası oluştu, lütfen daha sonra tekrar deneyiniz." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "İstek sırasında bir hata oluştu: " + errorMessage });
                    }
                }
                return Json(new { success = true });
            }
            catch (HttpRequestException ex)
            {

                return Json(new { success = false, message = $"Ağ hatası oluştu: {ex.Message}" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Beklenmedik bir hata oluştu: {ex.Message}" });
            }
        }
        public async Task<IActionResult> RequestStatus([FromQuery] string postId)
        {
            var userId = _userService.GetUserId();
            HttpResponseMessage response = await _apiservice.CheckIfRequestExistsAsync(postId, userId);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var requestStatus = Convert.ToInt32(jsonString);
                return Json(new { success = true, status = requestStatus });
            }
            return Json(new { success = false });
        }
        [HttpPut]
        public async Task<IActionResult> Confirm([FromQuery] string postId, [FromQuery] string userId)
        {

            HttpResponseMessage response = await _apiservice.Confirm(userId, postId);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPut]
        public async Task<IActionResult> Decline([FromQuery] string postId, [FromQuery] string userId)
        {

            HttpResponseMessage response = await _apiservice.Decline(userId, postId);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaxParticipants([FromForm] string postId, [FromForm] int maxParticipants)
        {
            var response = await _postapiservice.GetPostByIdAsync(postId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PostViewModel>();
                content.MaxParticipants = maxParticipants;
                HttpResponseMessage response2 = await _postapiservice.UpdatePost(content);
                if (response2.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = "Max participants updated successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Post not found" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Post not found" });
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] string participantId)
        {

            try
            {
                var response = await _apiservice.Delete(participantId);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Participant removed successfully" });
                }
                return Json(new { success = false, message = "Participant not found" });
            }
            catch (HttpRequestException ex)
            {

                return Json(new { success = false, message = $"Ağ hatası oluştu: {ex.Message}" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Beklenmedik bir hata oluştu: {ex.Message}" });
            }
        }



    }
}