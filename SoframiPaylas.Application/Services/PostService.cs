using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
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

        public async Task<string> CreatePostAsync(CreatePostDto postDto)
        {
            var post = new Post
            {
                UserID = postDto.UserID,
                Title = postDto.Title,
                Description = postDto.Description,
                Date = postDto.Date,
                Participants = postDto.Participants,
                Images = postDto.Images,
                Status = postDto.Status
            };
            return await _postrepository.CreatePostAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostAsync()
        {
            var posts = await _postrepository.GetAllPostsAsync();
            return posts.Select(p => new PostDto { Title = p.Title, Description = p.Description, UserID = p.UserID, Participants = p.Participants, Images = p.Images, Status = p.Status, FormattedDate = p.Date.ToDateTime().ToString("o") });
        }

        public async Task<PostDto> GetPostByIdAsync(string userId)
        {
            var post = await _postrepository.GetPostByIdAsync(userId);
            if (post == null)
            {
                throw new KeyNotFoundException($"No post found with ID {userId}");
            }
            return new PostDto
            {
                UserID = post.UserID,
                Title = post.Title,
                Description = post.Description,
                Participants = post.Participants,
                Images = post.Images,
                Status = post.Status,
                FormattedDate = post.Date.ToDateTime().ToString("o")
            };
        }

        public async Task UpdatePostAsync(UpdatePostDto postDto, string postId)
        {
            var post = new Post
            {
                PostID = postId,
                UserID = postDto.UserID,
                Title = postDto.Title,
                Description = postDto.Description,
                Date = postDto.Date,
                Participants = postDto.Participants,
                Images = postDto.Images,
                Status = postDto.Status
            };
            await _postrepository.UpdatePostAsync(post, postId);
        }
    }
}