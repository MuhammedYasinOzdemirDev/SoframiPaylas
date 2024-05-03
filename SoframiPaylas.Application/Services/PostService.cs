using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postrepository = postRepository;
            _mapper = mapper;
        }

        public async Task<string> CreatePostAsync(CreatePostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            return await _postrepository.CreatePostAsync(post);
        }

        public async Task DeletePostAsync(string postId)
        {
            var post = await _postrepository.GetPostByIdAsync(postId);
            if (post == null)
            {
                throw new Exception("Post not found.");
            }
            await _postrepository.DeletePostAsync(postId);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostAsync()
        {
            var posts = await _postrepository.GetAllPostsAsync();
            return posts.Select(p => _mapper.Map<PostDto>(p));
        }

        public async Task<PostDto> GetPostByIdAsync(string userId)
        {
            var post = await _postrepository.GetPostByIdAsync(userId);
            if (post == null)
            {
                throw new KeyNotFoundException($"No post found with ID {userId}");
            }
            return _mapper.Map<PostDto>(post);
        }

        public async Task UpdatePostAsync(UpdatePostDto postDto, string postId)
        {
            var post = await _postrepository.GetPostByIdAsync(postId);
            if (post == null)
            {
                throw new Exception("Post not found.");
            }
            _mapper.Map(postDto, post);
            await _postrepository.UpdatePostAsync(post, postId);
        }
    }
}