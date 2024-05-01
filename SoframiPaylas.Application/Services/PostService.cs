using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postrepository;

        public PostService(IPostRepository postRepository)
        {
            _postrepository = postRepository;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostAsync()
        {
            var posts = await _postrepository.GetAllPostsAsync();
            return posts.Select(p => new PostDto { Title = p.Title, Description = p.Description, UserID = p.UserID, Participants = p.Participants, Images = p.Images, Status = p.Status });
        }
    }
}