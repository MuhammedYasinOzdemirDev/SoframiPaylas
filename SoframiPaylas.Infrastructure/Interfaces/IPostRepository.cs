
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IPostRepository
    {
        Task<string> CreatePostAsync(Post post);
        Task<List<(Post post, string Id)>> GetPostAllAsync();
        Task<Post> GetPostByIdAsync(string id);
        Task<bool> UpdatePostAsync(string id, Post post);
        Task<bool> DeletePostAsync(string id);
        Task<List<(Post post, string postId)>> GetByUserIdPostAllAsync(string userId);

        Task<List<(Post post, string postId)>> GetPostsByIdsAsync(List<string> postIds);
        Task<bool> UpdateParticipantStatus(string postId, string participantId);
        Task<bool> RemoveParticipant(string participantId);
        Task<bool> EndPost(string postId);
    }

}