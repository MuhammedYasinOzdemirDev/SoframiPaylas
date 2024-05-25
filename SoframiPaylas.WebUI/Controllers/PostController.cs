
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.WebUI.ExternalService.Extensions;
using SoframiPaylas.WebUI.ExternalService.Filters;
using SoframiPaylas.WebUI.ExternalService.StorageService;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;


namespace SoframiPaylas.WebUI.Controllers
{
    [CustomAuthorize]
    public class PostController : Controller
    {
        private readonly IPostApiService _apiService;
        private readonly IUserService _userService;


        private static List<string> foodIdList = new List<string>();
        public PostController(IPostApiService apiService, IUserService userService)
        {
            _apiService = apiService;
            _userService = userService;
        }


        public IActionResult Share()
        {
            // Oturum verilerini temizle
            HttpContext.Session.Remove("FoodList");
            HttpContext.Session.Remove("FoodIdList");
            var foodList = new List<(string Title, string Id)>();
            ViewBag.FoodList = foodList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Share(CreatePostViewModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Model cannot be null." });
            }
            Console.WriteLine(model.FormattedDate);
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz form verisi" });
            }
            try

            {
                var foodList = HttpContext.Session.GetObjectFromJson<List<(string Title, string Id)>>("FoodList").Select(f => f.Id).ToList();
                var hostId = _userService.GetUserId();

                HttpResponseMessage response = await _apiService.CreatePost(model, foodList, hostId);
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
                HttpContext.Session.Remove("FoodList");
                return Json(new { success = true });
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
        public async Task<IActionResult> AddFood(string title, string description)
        {

            if (!string.IsNullOrEmpty(title))
            {
                if (string.IsNullOrEmpty(description))
                {
                    description = "Açıklama Yok";
                }
                var response = await _apiService.AddFood(title, description);
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
                var foodList = HttpContext.Session.GetObjectFromJson<List<(string Title, string Id)>>("FoodList") ?? new List<(string Title, string Id)>();

                foodList.Add((title, id));
                foodIdList.Add(id);

                HttpContext.Session.SetObjectAsJson("FoodList", foodList);


                var list = foodList.Select(food => new { Title = food.Title, Id = food.Id });

                return Json(new { success = true, foodList = list });
            }
            return Json(new { success = false, message = "Yemek adı boş olamaz" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFood(string foodId)
        {
            var response = await _apiService.RemoveFood(foodId);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return Json(new { success = false, message = "Silme sırasında bir hata oluştu: " + errorMessage });
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

            var foodList = HttpContext.Session.GetObjectFromJson<List<(string Title, string Id)>>("FoodList") ?? new List<(string Title, string Id)>();
            foodList.RemoveAll(f => f.Id == foodId);

            HttpContext.Session.SetObjectAsJson("FoodList", foodList);

            var list = foodList.Select(food => new { Title = food.Title, Id = food.Id });
            return Json(new { success = true, foodList = list });
        }
        public async Task<IActionResult> Manage()
        {
            var userId = _userService.GetUserId();

            var response = await _apiService.GetByUserIdPostAllAsync(userId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<PostViewModel>>();
                if (content.Count() == 0)
                {
                    return View(new List<PostViewModel>());
                }
                ViewBag.TotalShares = content.Count();
                ViewBag.TotalParticipation = content.Sum(x => x.Participants.Count());
                ViewBag.TotalFood = content.Sum(x => x.RelatedFoods.Count());
                return View(content);
            }
            return View("Error");
        }
        public async Task<IActionResult> Edit([FromQuery] string postId)
        {
            var response = await _apiService.GetPostByIdAsync(postId);
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }
            var content = await response.Content.ReadFromJsonAsync<PostViewModel>();

            var editViewModel = new PostEditViewModel()
            {
                Title = content.Title,
                Description = content.Description,
                Location = $"{content.Latitude.ToString(CultureInfo.InvariantCulture)}-{content.Longitude.ToString(CultureInfo.InvariantCulture)}",
                FormattedDate = content.FormattedDate,
                Time = content.Time,
                ImageUrl = content.Image
                ,
                PostId = content.PostId
            };
            return View(editViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromQuery] string postId, PostEditViewModel model)
        {
            Console.WriteLine(model.Title);
            Console.WriteLine(model.PostId);
            Console.WriteLine(model.ImageUrl);
            Console.WriteLine(model.Description);
            Console.WriteLine(model.Location);
            Console.WriteLine(model.Time);
            Console.WriteLine(model.FormattedDate);
            if (model == null)
            {
                return Json(new { success = false, message = "Model cannot be null." });
            }
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz form verisi" });
            }
            try
            {

                HttpResponseMessage response = await _apiService.GetPostByIdAsync(postId);
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false });

                }
                var locationParts = model.Location.Split('-');
                double latitude = double.Parse(locationParts[0], System.Globalization.CultureInfo.InvariantCulture);
                double longitude = double.Parse(locationParts[1], System.Globalization.CultureInfo.InvariantCulture);
                var content = await response.Content.ReadFromJsonAsync<PostViewModel>();
                content.Image = model.ImageUrl;
                content.Title = model.Title;
                content.Description = model.Description;
                content.Time = model.Time.ToString();
                content.Longitude = longitude;
                content.Latitude = latitude;
                content.FormattedDate = model.FormattedDate;
                var response2 = await _apiService.UpdatePost(content);
                if (response2.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });

            }
            catch (HttpRequestException ex)
            {
                return Json(new { success = false, message = $"Ağ hatası oluştu: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Beklenmedik bir hata oluştu: {ex.Message}" });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string postId)
        {
            try
            {
                var response = await _apiService.DeletePost(postId);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (HttpRequestException ex)
            {
                return Json(new { success = false, message = $"Ağ hatası oluştu: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Beklenmedik bir hata oluştu: {ex.Message}" });
            }
        }



    }
}