using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
                throw new ApplicationException($"An error occurred when retrieving events: {ex.Message}", ex);
            }
        }
    }
}