using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Food;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services
{
    public class PostApiService : IPostApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public PostApiService(IHttpClientFactory httpClient, IMapper mapper)
        {
            _httpClient = httpClient.CreateClient("API");
            _mapper = mapper;
        }
        public async Task<List<PostViewModel>> GetAllPostsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Post/posts");
                response.EnsureSuccessStatusCode(); // HTTP yanıtını kontrol et

                // API'den IEnumerable olarak gelen veriyi List'e çevir
                IEnumerable<PostDto> eventDtoEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<PostDto>>();
                List<PostDto> postList = eventDtoEnumerable.ToList();


                return _mapper.Map<List<PostViewModel>>(postList);
            }
            catch (HttpRequestException ex)
            {
                // RetryHandler tarafından yeniden denenmesine rağmen hala hata alınıyorsa
                Console.WriteLine($"An error occurred after retries: {ex.Message}");
                return null;  // Hata durumunda false dönerek, çağrı yapan koda bilgi ver
            }
            catch (Exception ex)
            {
                // Diğer beklenmedik hatalar için
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
        public async Task<HttpResponseMessage> AddFood(string title, string description)
        {

            var createFoodDto = new CreateFoodDto { Title = title, Description = description };
            var url = new Uri(_httpClient.BaseAddress + "Food/food");

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, createFoodDto);
            return response;
        }
        public async Task<HttpResponseMessage> RemoveFood(string foodId)
        {
            var url = new UriBuilder(_httpClient.BaseAddress + "Food/food")
            {
                Query = $"foodId={foodId}"
            };
            HttpResponseMessage response = await _httpClient.DeleteAsync(url.Uri);
            return response;
        }
        public async Task<HttpResponseMessage> CreatePost(CreatePostViewModel model, List<string> relatedFoods, string hostId)
        {
            var postDto = _mapper.Map<PostDto>(model);

            var locationParts = model.Location.Split(',');
            double latitude = double.Parse(locationParts[0], System.Globalization.CultureInfo.InvariantCulture);
            double longitude = double.Parse(locationParts[1], System.Globalization.CultureInfo.InvariantCulture);
            postDto.Latitude = latitude;
            postDto.Longitude = longitude;
            postDto.HostID = hostId;
            postDto.Image = model.ImageUrl;
            postDto.RelatedFoods = relatedFoods;
            postDto.Participants = new List<string>();
            var url = new Uri(_httpClient.BaseAddress + "Post/post");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, postDto);
            return response;
        }
        public async Task<HttpResponseMessage> GetPostByIdAsync(string postId)
        {

            var url = new UriBuilder(_httpClient.BaseAddress + "Post/post")
            {
                Query = $"postId={postId}"
            };
            HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
            return response;
        }
        public async Task<HttpResponseMessage> GetFoodByIdAsync(List<string> foodIds)
        {
            var url = new Uri(_httpClient.BaseAddress + "Food/foods");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, foodIds);
            return response;
        }

    }
}