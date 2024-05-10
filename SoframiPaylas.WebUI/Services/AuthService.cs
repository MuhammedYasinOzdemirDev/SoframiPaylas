using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public AuthService(IHttpClientFactory httpClient, IMapper mapper)
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
    }
}