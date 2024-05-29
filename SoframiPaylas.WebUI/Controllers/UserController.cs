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
        public async Task<IActionResult> Details([FromQuery] string postId)
        {
            ViewBag.UserId = _userService.GetUserId();

            var response = await _postApiService.GetPostByIdAsync(postId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PostViewModel>();
                var response2 = await _postApiService.GetFoodByIdAsync(content.RelatedFoods);

                IEnumerable<FoodViewModel> foodlist = await response2.Content.ReadFromJsonAsync<IEnumerable<FoodViewModel>>();

                if (foodlist != null)
                {
                    ViewBag.RelatedFoods = foodlist;
                }
                else
                {
                    ViewBag.RelatedFoods = new List<FoodViewModel>();

                }
                return View(content);
            }
            return View("Error");
        }
        public async Task<IActionResult> Manage()
        {

            try
            {
                List<PostViewModel> posts = new List<PostViewModel>();
                var userId = _userService.GetUserId();

                var response = await _postApiService.GetPostsByUserIdAsync(userId);
                if (response.IsSuccessStatusCode)
                {

                    List<string> postIds = await response.Content.ReadFromJsonAsync<List<string>>();
                    var response2 = await _postApiService.GetPostsByIdsAsync(postIds);

                    if (response2.IsSuccessStatusCode)
                    {
                        posts = await response2.Content.ReadFromJsonAsync<List<PostViewModel>>();
                        if (posts.Count == 0)
                        {
                            posts = new List<PostViewModel>();
                        }

                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    posts = new List<PostViewModel>();
                }
                var response3 = await _userApiService.MessageCount(userId);
                if (response3.IsSuccessStatusCode)
                {
                    var content = await response3.Content.ReadAsStringAsync();
                    ViewBag.MessageCount = content;
                }
                else
                {
                    ViewBag.MessageCount = "0";
                }
                var response4 = await _userApiService.CommentCount(userId);
                if (response4.IsSuccessStatusCode)
                {
                    var content = await response4.Content.ReadAsStringAsync();
                    ViewBag.CommentCount = content;
                }
                else
                {
                    ViewBag.CommentCount = "0";
                }

                return View(posts);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> Announcement()
        {
            var user = _userService.GetUser();
            ViewBag.ImageUrl = user.ProfilePicture;
            ViewBag.UserName = user.UserName;
            var response = await _postApiService.GetPostsByUserIdAsync(_userService.GetUserId());
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<string>>();
                var response2 = await _userApiService.GetAnnouncementPostIds(content);
                IEnumerable<AnnouncementView> announcements = new List<AnnouncementView>();
                if (response.IsSuccessStatusCode)
                {
                    announcements = await response2.Content.ReadFromJsonAsync<IEnumerable<AnnouncementView>>();

                }
                return View(announcements);
            }
            return View("Error");
        }
        public async Task<IActionResult> Message()
        {
            var user = _userService.GetUser();
            ViewBag.ImageUrl = user.ProfilePicture;
            ViewBag.UserName = user.UserName;
            var response = await _userApiService.GetMessageUserId(_userService.GetUserId());
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<MessageViewModel>>();

                return View(content);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View(new List<MessageViewModel>());
            }
            return View("Error");
        }
    }
}