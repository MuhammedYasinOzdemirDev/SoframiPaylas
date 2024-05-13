using AutoMapper;
using Newtonsoft.Json;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services;
public class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    public UserApiService(IHttpClientFactory httpClient, IMapper mapper)
    {
        _httpClient = httpClient.CreateClient("API");
        _mapper = mapper;
    }
    public async Task<UserProfileViewModel> GetUserProfileAsync(string userId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + $"User/users")
        {
            Query = $"userId={userId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        string jsonString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonString);
        var userDto = JsonConvert.DeserializeObject<UserDto>(jsonString);
        return _mapper.Map<UserProfileViewModel>(userDto);
    }
}