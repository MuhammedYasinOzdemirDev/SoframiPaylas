using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoframiPaylas.WebUI.Models;
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
            try
            {
                var postList = await _apiService.GetAllPostsAsync();
                if (postList == null)
                {
                    postList = new List<PostViewModel>();
                }
                return View(postList);
            }
            catch (HttpRequestException ex)
            {

                return null;  // Hata durumunda false dönerek, çağrı yapan koda bilgi ver
            }
            catch (Exception ex)
            {
                // Diğer beklenmedik hatalar için
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
        public async Task<IActionResult> Details(string postId)
        {
            try
            {
                var response = await _apiService.GetPostByIdAsync(postId);


                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                //var post = await response.Content.ReadFromJsonAsync<PostViewModel>();
                var jsonString = await response.Content.ReadAsStringAsync();


                var post = JsonConvert.DeserializeObject<PostViewModel>(jsonString);



                var response2 = await _apiService.GetFoodByIdAsync(post.RelatedFoods);

                IEnumerable<FoodViewModel> foodlist = await response2.Content.ReadFromJsonAsync<IEnumerable<FoodViewModel>>();

                if (foodlist != null)
                {
                    ViewBag.RelatedFoods = foodlist;
                }
                else
                {
                    ViewBag.RelatedFoods = new List<FoodViewModel>();

                }
                ViewBag.Participants = new List<string>();
                return View(post);
            }
            catch (HttpRequestException ex)
            {

                Console.WriteLine(ex);
                return View("Error");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return View("Error");
            }
        }
    }
}