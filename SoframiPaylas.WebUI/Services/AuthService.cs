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
        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var userDto = _mapper.Map<CreateUserDto>(model);
                // Query string oluşturma
                var url = new UriBuilder(_httpClient.BaseAddress + "api/Auth/register")
                {
                    Query = $"password={model.Password}"
                };

                // HTTP POST isteği gönderme
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url.ToString(), userDto);
                response.EnsureSuccessStatusCode(); // HTTP yanıtını kontrol et

                // İsteğin başarılı olduğunu konsola yazdır
                Console.WriteLine("User registered successfully!");
                return true;
            }
            catch (HttpRequestException ex)
            {
                // RetryHandler tarafından yeniden denenmesine rağmen hala hata alınıyorsa
                Console.WriteLine($"An error occurred after retries: {ex.Message}");
                return false;  // Hata durumunda false dönerek, çağrı yapan koda bilgi ver
            }
            catch (Exception ex)
            {
                // Diğer beklenmedik hatalar için
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
    }
}