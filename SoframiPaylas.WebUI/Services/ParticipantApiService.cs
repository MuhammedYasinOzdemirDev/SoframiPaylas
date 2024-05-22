using SoframiPaylas.Application.DTOs.Participant;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services;
public class ParticipantApiService : IParticipantApiService
{

    private readonly HttpClient _httpClient;

    public ParticipantApiService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient("API");
    }
    public async Task<HttpResponseMessage> Join(string userId, string postId)
    {
        var joinDto = new ParticipantDto
        {
            UserID = userId,
            PostID = postId
        };
        var url = new Uri(_httpClient.BaseAddress + "Post/join");

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, joinDto);
        return response;
    }
}