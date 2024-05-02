using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(string id);
        Task<string> CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post, string postId);
    }
}