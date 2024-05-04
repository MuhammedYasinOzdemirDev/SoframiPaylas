using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IPostRepository
    {
        Task<string> CreatePostAsync(Post post);
        Task<List<Post>> GetPostAllAsync();
        Task<Post> GetPostByIdAsync(string id);
        Task UpdatePostAsync(string id, Post post);
        Task DeletePostAsync(string id);

    }
}