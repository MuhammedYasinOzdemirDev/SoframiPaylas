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

        private readonly IEventApiService _eventApiService;
        public HomeController(IEventApiService eventApiService)
        {
            _eventApiService = eventApiService;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _eventApiService.GetAllEventsAsync();
            return View(events);
        }

    }
}