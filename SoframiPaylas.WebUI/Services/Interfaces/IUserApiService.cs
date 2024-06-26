using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface IUserApiService
    {
        Task<UserProfileViewModel> GetUserProfileAsync(string userId);
        Task<HttpResponseMessage> UpdateUserProfileAsync(UserProfileViewModel model, string userId);
        Task<HttpResponseMessage> MessageCount(string userId);
        Task<HttpResponseMessage> CommentCount(string userId);
        Task<HttpResponseMessage> GetUser(string userId);
        Task<HttpResponseMessage> GetAnnouncementPostIds(List<string> postIds);
        Task<HttpResponseMessage> GetMessageUserId(string userId);
    }
}