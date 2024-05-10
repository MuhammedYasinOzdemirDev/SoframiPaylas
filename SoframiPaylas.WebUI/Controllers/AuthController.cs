using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
                throw new ArgumentNullException(nameof(model), "Register model cannot be null.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await _authService.RegisterAsync(model);
                    // API'den hata mesajını oku

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        // Burada spesifik hata kodlarını ele alıyoruz
                        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            Console.WriteLine(errorMessage);

                            ModelState.AddModelError("", errorMessage);
                        }
                        else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                        {
                            // 500 ve üzeri hata kodları için genel bir işlem yapılabilir
                        }
                        else
                        {
                            ModelState.AddModelError("", errorMessage);
                        }
                        return View(model);
                    }

                    return RedirectToAction("Index", "Home");

                }
                catch (HttpRequestException ex)
                {
                    // RetryHandler tarafından yeniden denenmesine rağmen hala hata alınıyorsa
                    Console.WriteLine($"An error occurred after retries: {ex.Message}");
                    return View(model);// Hata durumunda false dönerek, çağrı yapan koda bilgi ver
                }
                catch (Exception ex)
                {
                    // Diğer beklenmedik hatalar için
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    return View(model);
                }
            }
            return View(model);
        }


    }
}