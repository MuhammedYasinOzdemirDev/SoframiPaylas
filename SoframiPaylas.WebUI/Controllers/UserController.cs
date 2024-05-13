using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiService _userApiService;
        public UserController(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Console.WriteLine(ClaimTypes.Email);
            Console.WriteLine("AA\n\n" + userId);
            return View(await _userApiService.GetUserProfileAsync(userId));
        }


    }
}