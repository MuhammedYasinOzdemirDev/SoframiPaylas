/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;*/
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.DTOs.Post;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IPostService
    {
        Task<string> CreatePostAsync(CreatePostDto postDto);
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(string id);
        Task<bool> UpdatePostAsync(string id, UpdatePostDto postDto);
        Task<bool> DeletePostAsync(string id);
    }
}