using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface IPostApiService
    {
        Task<List<PostViewModel>> GetAllPostsAsync();
        Task<HttpResponseMessage> AddFood(string title, string description);
        Task<HttpResponseMessage> CreatePost(CreatePostViewModel model, List<string> relatedFoods, string hostId);
        Task<HttpResponseMessage> GetPostByIdAsync(string postId);
        Task<HttpResponseMessage> GetFoodByIdAsync(List<string> foodIds);
        Task<HttpResponseMessage> RemoveFood(string foodId);
        Task<HttpResponseMessage> UpdatePost(PostViewModel model);
        Task<HttpResponseMessage> UpdateFood(string foodId, string title, string description);
        Task<HttpResponseMessage> GetByUserIdPostAllAsync(string userId);
        Task<HttpResponseMessage> DeletePost(string postId);
        Task<HttpResponseMessage> GetPostsByUserIdAsync(string userId);
        Task<HttpResponseMessage> GetPostsByIdsAsync(List<string> postIds);
        Task<HttpResponseMessage> Duyuru(string postId, string content);
        Task<HttpResponseMessage> Message(MessageViewModel messageViewModel);
        Task<HttpResponseMessage> DuyurulariGetir(string postId);
        Task<HttpResponseMessage> EndPost(string postId);
    }
}