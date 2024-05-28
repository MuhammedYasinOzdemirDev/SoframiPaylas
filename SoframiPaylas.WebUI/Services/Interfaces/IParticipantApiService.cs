using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface IParticipantApiService
    {
        Task<HttpResponseMessage> Join(string userId, string postId);
        Task<HttpResponseMessage> PendingParticipants(string postId);
        Task<HttpResponseMessage> ConfirmParticipants(string postId);
        Task<HttpResponseMessage> CheckIfRequestExistsAsync(string postId, string userId);
        Task<HttpResponseMessage> Decline(string userId, string postId);
        Task<HttpResponseMessage> Confirm(string userId, string postId);
        Task<HttpResponseMessage> Delete(string participantId);
        Task<HttpResponseMessage> GetUserIdPost(string userId);
        Task<HttpResponseMessage> GetUsersByPostId(string postId);
        Task<HttpResponseMessage> Leave(string postId, string userId);
    }
}