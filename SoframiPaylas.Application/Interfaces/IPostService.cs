using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Post;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostAsync();
        Task<PostDto> GetPostByIdAsync(string userId);
        Task<string> CreatePostAsync(CreatePostDto postDto);

    }
}