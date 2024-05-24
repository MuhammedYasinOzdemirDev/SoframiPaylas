using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.WebUI.ExternalService.Extensions;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Controllers
{

    public class FoodController : Controller
    {
        private readonly IPostApiService _apiService;

        private static List<string> foodIdList = new List<string>();
        public FoodController(IPostApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Post([FromQuery] string postId)
        {
            try
            {

                var response = await _apiService.GetPostByIdAsync(postId);
                if (response.IsSuccessStatusCode)
                {
                    PostViewModel content = await response.Content.ReadFromJsonAsync<PostViewModel>();
                    var response2 = await _apiService.GetFoodByIdAsync(content.RelatedFoods);
                    List<FoodViewModel> foodList;
                    if (response2.IsSuccessStatusCode)
                    {
                        foodList = await response2.Content.ReadFromJsonAsync<List<FoodViewModel>>();

                    }
                    else
                    {
                        foodList = new List<FoodViewModel>();
                    }

                    return View(new PostFoodListViewModel { PostId = postId, FoodList = foodList, PostName = content.Title });
                }
                else
                {
                    return View("Error");
                }
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

        [HttpPost]
        public async Task<IActionResult> AddFood([FromQuery] string postId, [FromBody] FoodViewModel model)
        {

            if (!string.IsNullOrEmpty(model.Title))
            {
                if (string.IsNullOrEmpty(model.Description))
                {
                    model.Description = "Açıklama Yok";
                }
                var response = await _apiService.AddFood(model.Title, model.Description);
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
                var id = await response.Content.ReadAsStringAsync();

                var response2 = await _apiService.GetPostByIdAsync(postId);
                if (response2.IsSuccessStatusCode)
                {
                    PostViewModel content = await response2.Content.ReadFromJsonAsync<PostViewModel>();
                    content.RelatedFoods.Add(id);
                    var response3 = await _apiService.UpdatePost(content);
                    if (response3.IsSuccessStatusCode)
                    {



                        return Json(new { success = true });
                    }
                }
                return Json(new { success = false });
            }
            return Json(new { success = false, message = "Yemek adı boş olamaz" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFood([FromQuery] string postId, [FromQuery] string foodId)
        {
            var response = await _apiService.RemoveFood(foodId);
            if (response.IsSuccessStatusCode)
            {
                var response2 = await _apiService.GetPostByIdAsync(postId);
                if (response2.IsSuccessStatusCode)
                {
                    PostViewModel content = await response2.Content.ReadFromJsonAsync<PostViewModel>();
                    content.RelatedFoods.Remove(foodId);
                    var response3 = await _apiService.UpdatePost(content);
                    if (response3.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            return Json(new { success = false });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateFood([FromQuery] string postId, [FromQuery] string foodId, [FromBody] FoodViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                if (string.IsNullOrEmpty(model.Description))
                {
                    model.Description = "Açıklama Yok";
                }
                var response = await _apiService.UpdateFood(foodId, model.Title, model.Description);
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
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Yemek adı boş olamaz" });
        }
    }
}