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
        var url = new Uri(_httpClient.BaseAddress + "Participant/join");

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, joinDto);
        return response;
    }
    public async Task<HttpResponseMessage> PendingParticipants(string postId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + "Participant/pending-participants")
        {
            Query = $"postId={postId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        return response;
    }
    public async Task<HttpResponseMessage> ConfirmParticipants(string postId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + "Participant/confirm-participants")
        {
            Query = $"postId={postId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        return response;
    }
    public async Task<HttpResponseMessage> CheckIfRequestExistsAsync(string postId, string userId)
    {

        var url = new UriBuilder(_httpClient.BaseAddress + "Participant/check-status")
        {
            Query = $"postId={postId}&userId={userId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        return response;
    }
    public async Task<HttpResponseMessage> Confirm(string userId, string postId)
    {
        var confirmDto = new ParticipantDto
        {
            UserID = userId,
            PostID = postId
        };
        var url = new Uri(_httpClient.BaseAddress + "Participant/confirm");

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, confirmDto);
        return response;
    }
    public async Task<HttpResponseMessage> Decline(string userId, string postId)
    {

        var declineDto = new ParticipantDto
        {
            UserID = userId,
            PostID = postId
        };
        var url = new Uri(_httpClient.BaseAddress + "Participant/decline");

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, declineDto);
        return response;
    }
    public async Task<HttpResponseMessage> Delete(string participantId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + "Participant/remove-participant")
        {
            Query = $"participantId={participantId}"
        };
        HttpResponseMessage response = await _httpClient.DeleteAsync(url.Uri);
        return response;
    }
    public async Task<HttpResponseMessage> GetUserIdPost(string userId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + "Post/get-user-posts")
        {
            Query = $"userId={userId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        return response;
    }
    public async Task<HttpResponseMessage> GetUsersByPostId(string postId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + "Participant/get-post-users")
        {
            Query = $"postId={postId}"
        };
        HttpResponseMessage response = await _httpClient.GetAsync(url.Uri);
        return response;
    }
    public async Task<HttpResponseMessage> Leave(string postId, string userId)
    {
        var url = new UriBuilder(_httpClient.BaseAddress + "Participant/leave-participant")
        {
            Query = $"postId={postId}&userId={userId}"
        };
        HttpResponseMessage response = await _httpClient.DeleteAsync(url.Uri);
        return response;
    }

}