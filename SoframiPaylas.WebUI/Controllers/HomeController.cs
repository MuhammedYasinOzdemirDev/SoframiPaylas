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
        private readonly IParticipantApiService _participantApiService;
        private readonly IUserApiService _userApiService;

        public HomeController(IPostApiService postApiService, IParticipantApiService participantApiService, IUserApiService userApiService)
        {
            _apiService = postApiService;
            _participantApiService = participantApiService;
            _userApiService = userApiService;
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
                var response3 = await _participantApiService.ConfirmParticipants(postId);
                Console.WriteLine(postId);
                if (response3.IsSuccessStatusCode)
                {
                    var participants = await response3.Content.ReadFromJsonAsync<List<ParticipantViewModel>>();
                    if (participants.Count == 0)
                        participants = new List<ParticipantViewModel>();
                    ViewBag.Participants = participants;
                }
                else
                {
                    var participants = new List<ParticipantViewModel>();
                    ViewBag.Participants = participants;
                }
                var response4 = await _userApiService.GetUser(post.HostID);
                if (response4.IsSuccessStatusCode)
                {
                    var host = await response4.Content.ReadFromJsonAsync<ProfileViewModel>();
                    ViewBag.Host = host;
                }
                else
                {
                    ViewBag.Host = null;
                }
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
        public async Task<IActionResult> Profile([FromQuery] string userId)
        {
            try
            {
                var response = await _userApiService.GetUser(userId);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<ProfileViewModel>(content);
                    var response2 = await _apiService.GetByUserIdPostAllAsync(userId);
                    if (response2.IsSuccessStatusCode)
                    {
                        var content2 = await response2.Content.ReadAsStringAsync();
                        var posts = JsonConvert.DeserializeObject<List<PostViewModel>>(content2);
                        ViewBag.CurrentPosts = posts.Where(p => p.PostStatus);
                        ViewBag.PastPosts = posts.Where(p => !p.PostStatus);
                    }
                    else
                    {
                        ViewBag.CurrentPosts = new List<PostViewModel>();
                        ViewBag.PastPosts = new List<PostViewModel>();
                    }
                    return View(user);
                }
                return View("Error");
            }
            catch (HttpRequestException ex)
            {

                return View("Error");
            }
            catch (Exception ex)
            {

                return View("Error");
            }

        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}