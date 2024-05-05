using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{

    public class HomeController : Controller
    {

        private readonly IPostApiService _apiService;
        public HomeController(IPostApiService postApiService)
        {
            _apiService = postApiService;
        }
        public async Task<IActionResult> Index()
        {
            var postList = await _apiService.GetAllPostsAsync();
            return View(postList);
        }

    }
}