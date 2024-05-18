using System.Text;
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
        var url = new UriBuilder(_httpClient.BaseAddress + "User/user")
        {
            Query = $"userId={userId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        string jsonString = await response.Content.ReadAsStringAsync();
        var userDto = JsonConvert.DeserializeObject<UserDto>(jsonString);
        return _mapper.Map<UserProfileViewModel>(userDto);
    }

    public async Task<HttpResponseMessage> UpdateUserProfileAsync(UserProfileViewModel model, string userId)
    {
        var updateDto = _mapper.Map<UpdateUserDto>(model);

        var url = new UriBuilder(_httpClient.BaseAddress + "User/user")
        {
            Query = $"userId={userId}"
        };
        var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(url.Uri, content);
        return response;
    }
    public async Task<HttpResponseMessage> UploadProfilePicture(byte[] file, string userId)
    {

        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Dosya geçersiz.");
        }

        var url = new UriBuilder(_httpClient.BaseAddress + "User/UploadProfilePicture")
        {
            Query = $"userId={userId}"
        };

        byte[] fileBytes;
        /*using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            fileBytes = ms.ToArray();
        }*/

        var content = new ByteArrayContent(file);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        // HTTP PUT isteği yapın
        HttpResponseMessage response = await _httpClient.PutAsync(url.Uri, content);
        return response;

    }

}