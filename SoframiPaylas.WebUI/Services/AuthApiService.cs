
using AutoMapper;
using Newtonsoft.Json;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.User;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services
{
    public class AuthApiService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public AuthApiService(IHttpClientFactory httpClient, IMapper mapper)
        {
            _httpClient = httpClient.CreateClient("API");
            _mapper = mapper;
        }
        public async Task<HttpResponseMessage> RegisterAsync(RegisterViewModel model)
        {

            var userDto = _mapper.Map<CreateUserDto>(model);
            // Query string oluşturma
            var url = new UriBuilder(_httpClient.BaseAddress + "Auth/register")
            {
                Query = $"password={model.Password}"
            };
            // HTTP POST isteği gönderme
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url.ToString(), userDto);
            return response;
        }
        public async Task<HttpResponseMessage> LoginAsync(LoginViewModel model)
        {
            var loginData = new LoginDto
            {
                Email = model.Email,
                Password = model.Password
            };
            var url = new Uri(_httpClient.BaseAddress + "Auth/login");

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, loginData);
            return response;
        }
        public async Task<FirebaseUser> VerifyUser(string IdToken)
        {

            var url = new UriBuilder(_httpClient.BaseAddress + "Auth/verify-user")
            {
                Query = $"idToken={IdToken}"
            };
            HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
            string jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FirebaseUser>(jsonString);
        }
    }
}