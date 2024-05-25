using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services
{
    public class CommentApiService : ICommentApiService
    {
        private readonly HttpClient _httpClient;

        public CommentApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId)
        {
            var response = await _httpClient.GetAsync($"Comment/{postId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<CommentDto>>();
        }

        public async Task<HttpResponseMessage> AddCommentAsync(CreateCommentDto commentDto)
        {
            return await _httpClient.PostAsJsonAsync("Comment", commentDto);
        }

        public async Task<HttpResponseMessage> DeleteCommentAsync(string commentId)
        {
            return await _httpClient.DeleteAsync($"Comment/{commentId}");
        }
    }
}
