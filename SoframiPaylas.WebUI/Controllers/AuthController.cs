using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoframiPaylas.WebUI.ExternalService.StorageService;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Model cannot be null." });
            }

            if (!ModelState.IsValid)
            {

                return Json(new { success = false, message = "Hata" });
            }
            try
            {
                HttpResponseMessage response = await _authService.RegisterAsync(model);

                // API'den başarısız yanıt gelmesi durumunda hataları işle
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return Json(new { success = false, message = "Kayıt sırasında bir hata oluştu: " + errorMessage });
                    }
                    else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                    {
                        // Sunucu hatası durumunda genel bir hata mesajı göster
                        return Json(new { success = false, message = "Bir sunucu hatası oluştu, lütfen daha sonra tekrar deneyiniz." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Kayıt sırasında bir hata oluştu: " + errorMessage });
                    }
                }

                // Başarılı kayıt sonrası JSON olarak başarı mesajı döndür
                return Json(new { success = true, message = "Kayıt başarılı! Lütfen giriş yapın.", redirectUrl = Url.Action("Index", "Home") });
            }
            catch (HttpRequestException ex)
            {
                // Ağ hatası durumunda hata mesajı ekle ve formu tekrar göster
                ModelState.AddModelError("", $"Ağ hatası oluştu: {ex.Message}");
                return Json(new { success = false, message = $"Ağ hatası oluştu: {ex.Message}" });
            }
            catch (Exception ex)
            {
                // Beklenmedik hatalar için hata mesajı ekle ve formu tekrar göster
                return Json(new { success = false, message = $"Beklenmedik bir hata oluştu: {ex.Message}" });
            }
        }
        public IActionResult EmailConfirmed()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null, string message = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!string.IsNullOrEmpty(message))
                TempData["ErrorMessage"] = message;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Model cannot be null." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Hata" });
            }
            try
            {
                HttpResponseMessage response = await _authService.LoginAsync(model);
                // API'den başarısız yanıt gelmesi durumunda hataları işle
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return Json(new { success = false, message = "Giriş sırasında bir hata oluştu: " + errorMessage });
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
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string token = jsonResponse?.token;



                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict, // Burada Lax da kullanılabilir, kullanım durumunuza bağlı.
                    Expires = DateTimeOffset.UtcNow.AddDays(1) // 1 gün sonra sona erecek
                };

                HttpContext.Response.Cookies.Append("AuthToken", token, cookieOptions);
                // Başarılı giriş sonrası JSON olarak başarı mesajı döndür
                if (returnUrl == null)
                {
                    returnUrl = Url.Action("Index", "Home");
                }
                return Json(new { success = true, message = "Giriş başarılı!", redirectUrl = returnUrl });
            }
            catch (HttpRequestException ex)
            {

                return Json(new { success = false, message = $"Ağ hatası oluştu: {ex.Message}" });
            }
            catch (Exception ex)
            {
                // Beklenmedik hatalar için hata mesajı ekle ve formu tekrar göster
                return Json(new { success = false, message = $"Beklenmedik bir hata oluştu: {ex.Message}" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {

            if (model == null)
            {
                return Json(new { message = "Please enter your current password and new password", success = false });
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return Json(new { message = "Parola eşleşmiyor", success = false });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var email = _userService.GetEmail();
                    var userId = _userService.GetUserId();
                    var request = new ChangePasswordRequest
                    {
                        Email = email,
                        UserId = userId,
                        NewPassword = model.NewPassword,
                        OldPassword = model.CurrentPassword
                    };
                    var response = await _authService.ChangePasswordAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = response.Content.ReadAsStringAsync().Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu: " });
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
                    var LoginViewModel = new LoginViewModel
                    {
                        Email = email,
                        Password = model.NewPassword
                    };
                    var response2 = await _authService.LoginAsync(LoginViewModel);
                    if (response2.IsSuccessStatusCode)
                    {
                        var responseContent = await response2.Content.ReadAsStringAsync();
                        dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string token = jsonResponse?.token;

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict, // Burada Lax da kullanılabilir, kullanım durumunuza bağlı.
                            Expires = DateTimeOffset.UtcNow.AddDays(1) // 1 gün sonra sona erecek
                        };

                        HttpContext.Response.Cookies.Append("AuthToken", token, cookieOptions);
                        return Json(new { success = true, message = "Paralo  başarıyla güncellendi...", isLogin = true });
                    }
                    else
                    {
                        return Json(new { success = true, message = "Paralo  başarıyla güncellendi...", isLogin = false });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { message = ex.Message, success = false });
                }
            }
            return Json(new { message = "Please enter your current password and new password", success = false });
        }
    }
}