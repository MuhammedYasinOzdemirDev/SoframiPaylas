using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services
{
    public class CommentApiService : ICommentApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CommentApiService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _mapper = mapper;
        }

        public async Task<HttpResponseMessage> GetCommentsByPostIdAsync(string postId)
        {
            var url = new UriBuilder(_httpClient.BaseAddress + "Comment")
            {
                Query = $"postId={postId}"
            };
            var response = await _httpClient.GetAsync(url.Uri);
            return response;
        }

        public async Task<HttpResponseMessage> AddCommentAsync(CreateCommentViewModel createCommentViewModel)
        {
            var dto = _mapper.Map<CreateCommentDto>(createCommentViewModel);
            var url = new Uri(_httpClient.BaseAddress + "Comment");
            return await _httpClient.PostAsJsonAsync(url, dto);
        }

        public async Task<HttpResponseMessage> DeleteCommentAsync(string commentId)
        {
            var url = new UriBuilder(_httpClient.BaseAddress + "Comment")
            {
                Query = $"commentId={commentId}"
            };
            return await _httpClient.DeleteAsync(url.Uri);
        }
    }
}
