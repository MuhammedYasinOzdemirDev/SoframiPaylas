using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SoframiPaylas.WebUI.ExternalService.Filters;
using SoframiPaylas.WebUI.ExternalService.StorageService;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserApiService _userApiService;
        private readonly IPostApiService _postApiService;

        public UserController(IUserService userService, IUserApiService userApiService, IPostApiService postApiService)
        {
            _userService = userService;
            _userApiService = userApiService;
            _postApiService = postApiService;
        }
        public IActionResult Profile()
        {
            var user = _userService.GetUser();

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUser();
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı." });
                }

                user.UserName = model.UserName;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.About = model.About;
                user.Phone = model.Phone;

                var response = await _userService.UpdateUser(user);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu: " + errorMessage });
                    }
                    else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                    {
                        // Sunucu hatası durumunda genel bir hata mesajı göster
                        return Json(new { success = false, message = "Bir sunucu hatası oluştu, lütfen daha sonra tekrar deneyiniz." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Giriş sırasında bir hata oluştu: " + errorMessage });
                    }
                }
                return Json(new { success = true, message = "Profil başarıyla güncellendi..." });
            }
            var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = "Lütfen tüm alanları doldurunuz", errors = errorList });
        }
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture([FromBody] UploadProfilePictureRequest request)
        {
            if (string.IsNullOrEmpty(request.FileUrl))
            {
                return Json(new { message = "Dosya URL'si alınamadı", success = false });
            }

            try
            {
                var user = _userService.GetUser();
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı." });
                }

                user.ProfilePicture = request.FileUrl;


                var response = await _userService.UpdateUser(user);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu: " + errorMessage });
                    }
                    else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                    {
                        return Json(new { success = false, message = "Bir sunucu hatası oluştu, lütfen daha sonra tekrar deneyiniz." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Yükleme sırasında bir hata oluştu: " + errorMessage });
                    }
                }
                return Json(new { success = true, message = "Profil resmi başarıyla güncellendi..." });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }


        public IActionResult Security()
        {
            var user = _userService.GetUser();
            var securitViewModel = new SecurityViewModel
            {
                UserName = user.Name,
                ProfilePicture = user.ProfilePicture
            };
            return View(securitViewModel);
        }
        public async Task<IActionResult> Details()
        {

            return View();
        }
        public async Task<IActionResult> Manage()
        {
            try
            {
                var userId = _userService.GetUserId();

                var response = await _postApiService.GetPostsByUserIdAsync(userId);
                if (response.IsSuccessStatusCode)
                {

                    List<string> postIds = await response.Content.ReadFromJsonAsync<List<string>>();
                    var response2 = await _postApiService.GetPostsByIdsAsync(postIds);
                    List<PostViewModel> posts;
                    if (response2.IsSuccessStatusCode)
                    {
                        posts = await response2.Content.ReadFromJsonAsync<List<PostViewModel>>();
                        if (posts.Count == 0)
                        {
                            posts = new List<PostViewModel>();
                        }
                        return View(posts);
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return View(new List<PostViewModel>());
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}