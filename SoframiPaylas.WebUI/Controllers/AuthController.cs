using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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
                return Json(new { success = true, message = "Giriş başarılı! Ana sayfaya yönlendiriliyorsunuz...", redirectUrl = Url.Action("Index", "Home") });
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

    }
}